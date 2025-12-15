using MediatR;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Commands
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}