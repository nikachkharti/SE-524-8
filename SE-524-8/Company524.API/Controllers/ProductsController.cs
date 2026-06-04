using Company524.API.Models.Product;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Company524.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpPost]
        [SwaggerRequestExample(typeof(ProductForCreatingDto), typeof(ProductForCreatingDtoExample))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreatingDto request)
        {
            await productService.CreateNewProductAsync(request);
            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var result = await productService.DeleteProductAsync(productId);
            if (result == 0)
                return NotFound();
            return Ok();
        }


        [HttpPut]
        [SwaggerRequestExample(typeof(ProductForUpdatingDto), typeof(ProductForUpdatingDtoExample))]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductForUpdatingDto request)
        {
            var result = await productService.UpdateProductAsync(request);
            if (result == 0)
                return NotFound();
            return Ok();
        }
    }
}
