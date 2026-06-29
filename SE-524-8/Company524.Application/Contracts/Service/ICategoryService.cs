using Company524.Application.Models.Category;
using Company524.Application.Models.Common;

namespace Company524.Application.Contracts.Service
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
