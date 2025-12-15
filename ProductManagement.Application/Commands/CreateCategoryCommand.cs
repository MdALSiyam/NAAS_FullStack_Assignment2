using MediatR;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Commands
{
    public class CreateCategoryCommand : IRequest<string>
    {
        public CategoryDto CategoryDto { get; set; }
    }
}