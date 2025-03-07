using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.DTOs;
using WP.Services;

namespace WP.Web.Controllers
{
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            ViewBag.Categories = categories.Data;
            return View(categories);    
        }
        public async Task<IActionResult> EditCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryRequestDto category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                TempData["ErrorMessage"] = "Category name is required!";
                return RedirectToAction("Index");
            }
            try
            {
                await _categoryService.AddCategoryAsync(category);
                TempData["SuccessMessage"] = "Category added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the category: " + ex.Message;
            }

            return RedirectToAction("Index");
        }


        [HttpPost("delete")]
        public async Task<IActionResult> DeleteCategory([FromBody] ulong id)
        {
            
            return Ok();
        }


    }
}
