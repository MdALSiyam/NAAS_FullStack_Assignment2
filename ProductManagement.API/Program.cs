using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Queries;
using ProductManagement.Core.Interfaces;
using ProductManagement.Infrastructure.Repositories;
using Serilog;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;
using System;
using ProductManagement.API.Middlewares;
using Microsoft.AspNetCore.RateLimiting;
using FluentValidation.AspNetCore;
using Mapster;
using ProductManagement.Core.Entities;
using ProductManagement.Application.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Serilog (write to MongoDB sink using the configured connection string)
builder.Host.UseSerilog((ctx, lc) =>
    lc.Enrich.FromLogContext()
      .WriteTo.Console()
      .WriteTo.MongoDB(ctx.Configuration["MongoSettings:ConnectionString"], collectionName: "apilogs")
);

// Add services
builder.Services.AddControllers()
    .AddFluentValidation(cfg =>
    {
        cfg.RegisterValidatorsFromAssemblyContaining<ProductManagement.Application.Validators.ProductDtoValidator>();
        cfg.RegisterValidatorsFromAssemblyContaining<ProductManagement.Application.Validators.CategoryDtoValidator>();
    });

// Mapster configuration
TypeAdapterConfig.GlobalSettings.Scan(typeof(ProductManagement.Application.Profiles.MappingProfile).Assembly);

// Swagger + JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductManagement API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// API versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
});

// Redis cache
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration["Redis:Connection"];
});

// MongoDB registration (DI for IMongoDatabase)
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration["MongoSettings:ConnectionString"]));
builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var mongoUrl = new MongoUrl(builder.Configuration["MongoSettings:ConnectionString"]);
    var dbName = mongoUrl.DatabaseName ?? "ProductDb";
    return client.GetDatabase(dbName);
});

// Authentication (JWT)
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

// Ensure key is at least 256 bits (32 bytes) for HS256
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
if (keyBytes.Length < 32)
{
    throw new InvalidOperationException($"Jwt:Key must be at least 256 bits (32 bytes). Current key is {keyBytes.Length * 8} bits. Use a longer, secure secret stored in environment/user-secrets/KeyVault.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
            ValidIssuer = jwtIssuer,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

// Rate limiting (simple fixed-window)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opts =>
    {
        opts.PermitLimit = 100;
        opts.Window = TimeSpan.FromMinutes(1);
        opts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opts.QueueLimit = 0;
    });
});

// DI: MediatR, repository
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductsQuery).Assembly));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

// Global exception handling middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("fixed");

// Redirect root to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
