using Company524.API.Entities;
using Company524.API.Models.Category;
using Company524.API.Models.Product;
using Mapster;

namespace Company524.API.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Category, CategoryForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.CategoryName, src => src.CategoryName);

            config.NewConfig<ProductForCreatingDto, Product>()
                .Map(dest => dest.ProductName, src => src.ProductName)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.SupplierId, src => src.SupplierId);


            config.NewConfig<Product, ProductForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ProductName, src => src.ProductName)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Category, src => src.Category.CategoryName);


            config.NewConfig<ProductForUpdatingDto, Product>()
                .Map(dest => dest.ProductName, src => src.ProductName)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.SupplierId, src => src.SupplierId);
        }
    }
}
