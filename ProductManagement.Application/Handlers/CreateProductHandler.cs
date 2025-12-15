using MediatR;
using ProductManagement.Application.Commands;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, string>
    {
        private readonly IProductRepository _repo;

        public CreateProductHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.ProductDto.Name,
                Price = request.ProductDto.Price,
                CategoryId = request.ProductDto.CategoryId
            };
            await _repo.CreateAsync(product);
            return product.Id;
        }
    }
}