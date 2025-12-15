using MediatR;
using ProductManagement.Application.Commands;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductRepository _repo;

        public UpdateProductHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = request.Id,
                Name = request.ProductDto.Name,
                Price = request.ProductDto.Price,
                CategoryId = request.ProductDto.CategoryId
            };
            await _repo.UpdateAsync(product);
            return Unit.Value;
        }
    }
}