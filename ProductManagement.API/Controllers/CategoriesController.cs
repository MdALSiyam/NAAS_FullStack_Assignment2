using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Commands;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Queries;
using ProductManagement.Core.Entities;
using Asp.Versioning;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    [ApiVersion("1.0")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            var dtos = categories.Adapt<List<CategoryDto>>();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery { Id = id });
            if (category == null) return NotFound();
            return Ok(category.Adapt<CategoryDto>());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            var id = await _mediator.Send(new CreateCategoryCommand { CategoryDto = dto });
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(string id, [FromBody] CategoryDto dto)
        {
            await _mediator.Send(new UpdateCategoryCommand { Id = id, CategoryDto = dto });
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id = id });
            return NoContent();
        }
    }
}