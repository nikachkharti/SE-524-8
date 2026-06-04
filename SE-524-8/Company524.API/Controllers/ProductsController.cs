using Company524.API.Entities;
using Company524.API.Models.Product;
using Company524.API.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Company524.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController(IProductRepository productRepository) : ControllerBase
    {
        [HttpPost]
        [SwaggerRequestExample(typeof(ProductForCreatingDto), typeof(ProductForCreatingDtoExample))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreatingDto request)
        {
            //Mapping
            await productRepository.AddAsync(new Product()
            {
                ProductName = request.ProductName,
                Price = request.Price,
                Quantity = request.Quantity,
                CategoryId = request.CategoryId,
                SupplierId = request.SupplierId
            });
            await productRepository.SaveAsync();
            return Created();
        }
    }
}
