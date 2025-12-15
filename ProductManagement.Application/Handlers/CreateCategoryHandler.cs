using MediatR;
using ProductManagement.Application.Commands;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, string>
    {
        private readonly ICategoryRepository _repo;

        public CreateCategoryHandler(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.CategoryDto.Name
            };
            await _repo.CreateAsync(category);
            return category.Id;
        }
    }
}