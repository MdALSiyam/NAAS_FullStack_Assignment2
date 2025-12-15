using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Core.Entities;

namespace ProductManagement.Application.Queries
{
    public record GetProductsQuery() : IRequest<List<Product>>;
}
