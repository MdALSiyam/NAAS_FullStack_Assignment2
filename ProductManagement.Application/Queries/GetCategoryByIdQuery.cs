using MediatR;
using ProductManagement.Core.Entities;

namespace ProductManagement.Application.Queries
{
    public class GetCategoryByIdQuery : IRequest<Category>
    {
        public string Id { get; set; }
    }
}