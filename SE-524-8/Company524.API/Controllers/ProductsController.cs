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
        /// <summary>
        /// ახალი პროდუქტის დამატება
        /// </summary>
        [HttpPost]
        [SwaggerRequestExample(typeof(ProductForCreatingDto), typeof(ProductForCreatingDtoExample))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreatingDto request)
        {
            await productService.CreateNewProductAsync(request);
            return Created();
        }

        /// <summary>
        /// ყველა პროდუქტის აღება
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// კონკრეტული პროდუქტის აღება მისი იდენტიფიკატორის მიხედვით
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            var product = await productService.GetProductAsync(id);
            return Ok(product);
        }

        /// <summary>
        /// პროდუქტის წაშლა
        /// </summary>
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var result = await productService.DeleteProductAsync(productId);
            if (result == 0)
                return NotFound();
            return Ok();
        }

        /// <summary>
        /// პროდუქტის განახლება
        /// </summary>
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
