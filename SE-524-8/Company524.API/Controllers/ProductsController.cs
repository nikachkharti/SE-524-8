using Company524.API.Entities;
using Company524.API.Models.Product;
using Company524.API.Repository.Contracts;
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
    }
}
