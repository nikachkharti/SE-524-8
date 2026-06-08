using Company524.API.Models.Category;
using Company524.API.Models.Common;

namespace Company524.API.Service.Contracts
{
    public interface ICategoryService
    {
        Task<PagedResponseDto<CategoryForGettingDto>> GetAllCategoriesAsync(PagedRequestDto parameters);
        Task<CategoryForGettingDto> GetCategoryByIdAsync(Guid id);
        Task<Guid> CreateCategoryAsync(CategoryForCreatingDto model);
        Task<CategoryForGettingDto> UpdateCategoryAsync(CategoryForUpdatingDto model);
        Task DeleteCategoryAsync(Guid id);
    }
}
