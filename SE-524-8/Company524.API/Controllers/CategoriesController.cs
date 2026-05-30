using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Company524.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private static List<string> _categories = new List<string>()
        {
            "Electronics",
            "Books",
            "Clothing",
            "Home & Kitchen",
            "Sports & Outdoors"
        };

        [HttpPost]
        public IActionResult AddCategory([FromBody] string category)
        {
            _categories.Add(category);
            return Created(); //201
        }


        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok(_categories); //200
        }


        [HttpGet("single/{categoryName}")]
        public IActionResult GetSingleCategory([FromRoute] string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return BadRequest("Category name is required"); //400

            var category = _categories.FirstOrDefault(c => c.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category == null)
                return NotFound("Category not found"); //404


            return Ok(category);
        }


        [HttpPut("{categoryName}")]
        public IActionResult UpdateCategory([FromRoute] string categoryName, [FromBody] string newCategoryName)
        {
            var index = _categories.IndexOf(categoryName);
            if (index != -1)
            {
                _categories[index] = newCategoryName;
                return Ok($"Category '{categoryName}' updated to '{newCategoryName}' successfully.");
            }
            return NotFound($"Category '{categoryName}' not found.");
        }


        [HttpDelete("{categoryName}")]
        public IActionResult DeleteCategory([FromRoute] string categoryName)
        {
            var removed = _categories.Remove(categoryName);
            if (removed)
            {
                return Ok($"Category '{categoryName}' deleted successfully.");
            }
            return NotFound($"Category '{categoryName}' not found.");
        }

    }
}
