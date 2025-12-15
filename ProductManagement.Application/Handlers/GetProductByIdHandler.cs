using MediatR;
using ProductManagement.Application.Queries;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _repo;

        public GetProductByIdHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id);
        }
    }
}