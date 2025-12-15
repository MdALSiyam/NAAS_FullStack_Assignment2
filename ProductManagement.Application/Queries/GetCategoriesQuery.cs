using MediatR;
using ProductManagement.Core.Entities;
using System.Collections.Generic;

namespace ProductManagement.Application.Queries
{
    public class GetCategoriesQuery : IRequest<List<Category>>
    {
    }
}