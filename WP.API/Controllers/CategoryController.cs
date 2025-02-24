using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using WP.Data;
using WP.DTOs;
using WP.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpPost("create")]

        public async Task<IActionResult> CreateCategory(CategoryRequestDto category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return BadRequest(new { message = "Name of category is required" });
            }
            var createCategory = await _categoryService.AddCategoryAsync(category);
            return Ok(new { message = "Category created successfully", category });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            return Ok(categories);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCategory(List<ulong> Ids)
        {
            if (Ids.Count == 0 || Ids == null)
                return BadRequest("IDs cannot be empty.");
            await _categoryService.DeleteCategoryAsync(Ids);
            return Ok("Users deleted successfully.");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory(CategoryDto category)
        {
            if (category == null || category.Id == 0)
                return BadRequest(new { message = "Invalid data" });

            bool isUpdated = await _categoryService.UpdateCategoryAsync(category);

            if (!isUpdated)
                return NotFound(new { message = "Category not found" });

            return Ok(new { message = "Category updated successfully" });
        }

        [HttpPut("quick-update")]
        public async Task<IActionResult> QuickUpdateCategory(WpTerm category)
        {
            if (category == null || category.TermId == 0)
                return BadRequest(new { message = "Invalid data" });

            await _categoryService.QuickUpdateCategoryAsync(category);
            return Ok(new { message = "Category updated successfully" });
        }

    }
}
