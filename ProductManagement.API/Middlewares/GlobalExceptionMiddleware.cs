using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductManagement.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the exception middleware will rethrow.");
                    throw;
                }

                context.Response.Clear();
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = new ProblemDetails
                {
                    Type = "https://httpstatuses.io/500",
                    Title = "An unexpected error occurred.",
                    Status = context.Response.StatusCode,
                    Detail = _env.IsDevelopment() ? ex.ToString() : "An internal error occurred. Please contact support."
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var payload = JsonSerializer.Serialize(problem, options);

                await context.Response.WriteAsync(payload);
            }
        }
    }
}