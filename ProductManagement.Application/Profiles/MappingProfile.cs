using Mapster;
using ProductManagement.Application.DTOs;
using ProductManagement.Core.Entities;

namespace ProductManagement.Application.Profiles
{
    public class MappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Product, ProductDto>();
            config.NewConfig<ProductDto, Product>()
                  .IgnoreNullValues(true);

            config.NewConfig<Category, CategoryDto>();
            config.NewConfig<CategoryDto, Category>()
                  .IgnoreNullValues(true);
        }
    }
}