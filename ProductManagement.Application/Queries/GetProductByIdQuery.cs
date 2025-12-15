using MediatR;
using ProductManagement.Core.Entities;

namespace ProductManagement.Application.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public string Id { get; set; }
    }
}