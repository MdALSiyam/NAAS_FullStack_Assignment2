using MediatR;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Commands
{
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}