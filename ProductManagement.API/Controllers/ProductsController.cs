using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ProductManagement.Application.Commands;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Queries;
using ProductManagement.Core.Entities;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Asp.Versioning;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDistributedCache _cache;

        public ProductsController(IMediator mediator, IDistributedCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cached = await _cache.GetStringAsync("products");
            if (cached != null)
                return Ok(JsonSerializer.Deserialize<List<ProductDto>>(cached));

            var products = await _mediator.Send(new GetProductsQuery());
            var dtos = products.Adapt<List<ProductDto>>();

            await _cache.SetStringAsync("products", JsonSerializer.Serialize(dtos));

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null) return NotFound();

            return Ok(product.Adapt<ProductDto>());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            var id = await _mediator.Send(new CreateProductCommand { ProductDto = dto });
            await _cache.RemoveAsync("products");
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] ProductDto dto)
        {
            await _mediator.Send(new UpdateProductCommand { Id = id, ProductDto = dto });
            await _cache.RemoveAsync("products");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteProductCommand { Id = id });
            await _cache.RemoveAsync("products");
            return NoContent();
        }
    }
}
