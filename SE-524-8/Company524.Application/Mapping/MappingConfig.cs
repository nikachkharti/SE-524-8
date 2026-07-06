using Company524.Application.Models.Authentication;
using Company524.Application.Models.Category;
using Company524.Application.Models.Product;
using Company524.Application.Models.ProductImage;
using Company524.Application.Models.Supplier;
using Company524.Domain.Entities;
using Mapster;

namespace Company524.Application.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Category, CategoryForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.CategoryName, src => src.CategoryName);
            config.NewConfig<CategoryForCreatingDto, Category>();
            config.NewConfig<CategoryForUpdatingDto, Category>();


            config.NewConfig<Supplier, SupplierForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.SupplierName, src => src.SupplierName);
            config.NewConfig<SupplierForCreatingDto, Supplier>();
            config.NewConfig<SupplierForUpdatingDto, Supplier>();

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
                .Map(dest => dest.Category, src => src.Category);


            config.NewConfig<Product, ProductListForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ProductName, src => src.ProductName)
                .Map(dest => dest.Price, src => src.Price);


            config.NewConfig<ProductForUpdatingDto, Product>()
                .Map(dest => dest.ProductName, src => src.ProductName)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.Quantity, src => src.Quantity)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.SupplierId, src => src.SupplierId);

            config.NewConfig<RegistrationRequestDto, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper())
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.NormalizedEmail, src => src.Email.ToUpper());


            config.NewConfig<ProductImage, ProductImageForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.ImagePublicId, src => src.ImagePublicId);

            config.NewConfig<ProductImageForCreatingDto, ProductImage>()
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.ImageUrl, src => src.File.FileName)
                .Map(dest => dest.ImagePublicId, src => src.File.FileName);

            config.NewConfig<ProductImageForUpdatingDto, ProductImage>()
                .Map(dest => dest.ProductId, src => src.ExistingImageId)
                .Map(dest => dest.ImageUrl, src => src.File.FileName)
                .Map(dest => dest.ImagePublicId, src => src.File.FileName);
        }
    }
}
