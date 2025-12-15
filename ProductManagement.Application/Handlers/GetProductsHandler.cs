using MediatR;
using ProductManagement.Application.Queries;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly IProductRepository _repo;

        public GetProductsHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
