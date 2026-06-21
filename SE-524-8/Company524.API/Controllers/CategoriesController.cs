using Company524.API.Models.Category;
using Company524.API.Models.Common;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Company524.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController(IProductService productService, ICategoryService categoryService) : ControllerBase
    {
        /// <summary>
        /// პროდუქტები კონკრეტული კატეგორიების მიხედვით
        /// </summary>
        [HttpGet("{categoryId}/products")]
        public async Task<IActionResult> GetCategoryProducts(
            [FromRoute] Guid categoryId,
            [FromQuery] PagedRequestDto parameters)
        {
            var result = await productService.GetAllProductsOfCategoryAsync(categoryId, parameters);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };

            return StatusCode(response.HttpStatusCode, response);
        }

        /// <summary>
        /// ყველა კატეგორია
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] PagedRequestDto parameters)
        {
            var result = await categoryService.GetAllCategoriesAsync(parameters);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };

            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// კონკრეტული კატეგორია, Id პარამეტრით
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var result = await categoryService.GetCategoryByIdAsync(id);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };

            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// ახალი კატეგორიის დამატება
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerRequestExample(typeof(CategoryForCreatingDto), typeof(CategoryForCreatingDtoExample))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryForCreatingDto model)
        {
            var result = await categoryService.CreateCategoryAsync(model);
            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = result
            };
            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// არსებული კატეგორიის წაშლა
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await categoryService.DeleteCategoryAsync(id);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK)
            };
            return StatusCode(response.HttpStatusCode, response);
        }


        /// <summary>
        /// არსებული კატეგორიის განახლება
        /// </summary>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [SwaggerRequestExample(typeof(CategoryForUpdatingDto), typeof(CategoryForUpdatingDtoExample))]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryForUpdatingDto model)
        {
            var result = await categoryService.UpdateCategoryAsync(model);

            var response = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };
            return StatusCode(response.HttpStatusCode, response);
        }

    }
}
