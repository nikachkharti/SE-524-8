using Company524.API.Models.Common;
using Company524.API.Models.Product;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Company524.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        /// <summary>
        /// ახალი პროდუქტის დამატება
        /// </summary>
        [HttpPost]
        [SwaggerRequestExample(typeof(ProductForCreatingDto), typeof(ProductForCreatingDtoExample))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreatingDto request)
        {
            var result = await productService.CreateNewProductAsync(request);
            var response = new CommonResponse(CommonResponseMessage.SuccessMessage, result, true, Convert.ToInt32(HttpStatusCode.Created));

            return StatusCode(response.HttpStatusCode, response);
        }

        /// <summary>
        /// ყველა პროდუქტის აღება
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] PagedRequestDto parameters)
        {
            var result = await productService.GetAllProductsAsync(parameters);
            var response = new CommonResponse(
                CommonResponseMessage.SuccessMessage,
                result,
                true,
                Convert.ToInt32(HttpStatusCode.OK)
            );

            return StatusCode(response.HttpStatusCode, response);
        }

        /// <summary>
        /// კონკრეტული პროდუქტის აღება მისი იდენტიფიკატორის მიხედვით
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            var result = await productService.GetProductAsync(id);
            var response = new CommonResponse(CommonResponseMessage.SuccessMessage, result, true, Convert.ToInt32(HttpStatusCode.OK));

            return StatusCode(response.HttpStatusCode, response);
        }

        /// <summary>
        /// პროდუქტის წაშლა
        /// </summary>
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var result = await productService.DeleteProductAsync(productId);
            var response = new CommonResponse(CommonResponseMessage.SuccessMessage, result, true, Convert.ToInt32(HttpStatusCode.NoContent));

            return StatusCode(response.HttpStatusCode, response);
        }

        /// <summary>
        /// პროდუქტის განახლება
        /// </summary>
        [HttpPut]
        [SwaggerRequestExample(typeof(ProductForUpdatingDto), typeof(ProductForUpdatingDtoExample))]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductForUpdatingDto request)
        {
            var result = await productService.UpdateProductAsync(request);
            var response = new CommonResponse(CommonResponseMessage.SuccessMessage, result, true, Convert.ToInt32(HttpStatusCode.OK));

            return StatusCode(response.HttpStatusCode, response);
        }
    }
}
