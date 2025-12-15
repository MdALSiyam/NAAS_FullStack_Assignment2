using MediatR;
using ProductManagement.Application.Queries;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
    {
        private readonly ICategoryRepository _repo;

        public GetCategoriesHandler(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}