using Company524.API.Models.Common;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Company524.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController(IProductService productService) : ControllerBase
    {
        [HttpGet("{categoryId}/products")]
        public async Task<IActionResult> GetCategoryProducts([FromRoute] Guid categoryId, [FromQuery] PagedRequestDto parameters)
        {
            throw new NotImplementedException();
        }
    }
}
