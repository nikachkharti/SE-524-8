using Microsoft.AspNetCore.Mvc;

namespace Company524.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //private static List<string> _categories = new List<string>()
        //{
        //    "Electronics",
        //    "Books",
        //    "Clothing",
        //    "Home & Kitchen",
        //    "Sports & Outdoors"
        //};


        [HttpPost]
        public string AddCategory([FromBody] string category)
        {
            _categories.Add(category);
            return $"Category '{category}' added successfully.";
        }


        [HttpGet]
        public IEnumerable<string> GetAllCategories()
        {
            return _categories;
        }


        [HttpGet("single/{categoryName}")]
        public string GetSingleCategory([FromRoute] string categoryName)
        {
            var category = _categories.FirstOrDefault(c => c.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            return category;
        }


        [HttpPut("{categoryName}")]
        public string UpdateCategory([FromRoute] string categoryName, [FromBody] string newCategoryName)
        {
            var index = _categories.IndexOf(categoryName);
            if (index != -1)
            {
                _categories[index] = newCategoryName;
                return $"Category '{categoryName}' updated to '{newCategoryName}' successfully.";
            }
            return $"Category '{categoryName}' not found.";
        }


        [HttpDelete("{categoryName}")]
        public string DeleteCategory([FromRoute] string categoryName)
        {
            var removed = _categories.Remove(categoryName);
            if (removed)
            {
                return $"Category '{categoryName}' deleted successfully.";
            }
            return $"Category '{categoryName}' not found.";
        }

    }
}
