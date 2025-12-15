using MediatR;
using ProductManagement.Application.Commands;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _repo;

        public UpdateCategoryHandler(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Id = request.Id,
                Name = request.CategoryDto.Name
            };
            await _repo.UpdateAsync(category);
            return Unit.Value;
        }
    }
}