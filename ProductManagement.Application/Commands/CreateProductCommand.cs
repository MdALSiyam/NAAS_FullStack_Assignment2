using MediatR;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Commands
{
    public class CreateProductCommand : IRequest<string>
    {
        public ProductDto ProductDto { get; set; }
    }
}