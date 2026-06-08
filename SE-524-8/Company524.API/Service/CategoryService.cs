using Company524.API.Entities;
using Company524.API.Exceptions;
using Company524.API.Models.Category;
using Company524.API.Models.Common;
using Company524.API.Repository.Contracts;
using Company524.API.Service.Contracts;
using MapsterMapper;
using System.Linq.Expressions;

namespace Company524.API.Service
{
    public class CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : ICategoryService
    {
        public async Task<Guid> CreateCategoryAsync(CategoryForCreatingDto model)
        {
            if (model is null)
                throw new BadRequestException($"Request model is required");

            if (model.CategoryName.Length > 50)
                throw new BadRequestException("Category name length can't exceed 50");

            var category = mapper.Map<Category>(model);
            await categoryRepository.AddAsync(category);
            await categoryRepository.SaveAsync();
            return category.Id;
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Product category id is required");

            var category = await categoryRepository.GetAsync(c => c.Id == id);

            if (category is null)
                throw new NotFoundException("Category not found");

            categoryRepository.Remove(category);
            await categoryRepository.SaveAsync();
        }

        public async Task<PagedResponseDto<CategoryForGettingDto>> GetAllCategoriesAsync(PagedRequestDto parameters)
        {
            Expression<Func<Category, object>> orderBy = parameters.SortBy?.ToLower() switch
            {
                "categoryname" => p => p.CategoryName,
                _ => p => p.Id
            };

            var categories = await categoryRepository.GetAllAsync(
                orderBy: orderBy,
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            if (!categories.Items.Any())
                return new PagedResponseDto<CategoryForGettingDto>
                {
                    Items = Enumerable.Empty<CategoryForGettingDto>(),
                    TotalCount = 0,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };

            var result = mapper.Map<IEnumerable<CategoryForGettingDto>>(categories.Items);
            return new PagedResponseDto<CategoryForGettingDto>
            {
                Items = result,
                TotalCount = categories.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public async Task<CategoryForGettingDto> GetCategoryByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new BadRequestException("Category id is required");

            var category = await categoryRepository.GetAsync(filter: c => c.Id == id);

            if (category is null)
                throw new NotFoundException($"Category with id: {id} not found");

            return mapper.Map<CategoryForGettingDto>(category);
        }

        public async Task<CategoryForGettingDto> UpdateCategoryAsync(CategoryForUpdatingDto model)
        {
            if (model is null)
                throw new BadRequestException($"Request model is required");

            if (model.Id == Guid.Empty)
                throw new BadRequestException("Category id is required");

            if (model.CategoryName.Length > 50)
                throw new BadRequestException("Category name length can't exceed 50");

            var category = await categoryRepository.GetAsync(filter: c => c.Id == model.Id);

            if (category is null)
                throw new NotFoundException($"Category with id: {model.Id} not found");

            mapper.Map(model, category);
            categoryRepository.Update(category);
            await categoryRepository.SaveAsync();
            return mapper.Map<CategoryForGettingDto>(category);
        }
    }
}
