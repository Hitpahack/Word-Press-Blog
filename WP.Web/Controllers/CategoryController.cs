using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.DTOs;
using WP.EDTOs.Categories;
using WP.Service.Categories;
using WP.Services;

namespace WP.Web.Controllers
{
    [AllowAnonymous]
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ITermsService _termsService;
        public CategoryController(ICategoryService categoryService, IMapper mapper, ITermsService termsService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _termsService = termsService;
        }
        public async Task<IActionResult> Index()
        {
            var parentCategories = await _termsService.GetParentCategories();
            ViewBag.ParentCategories = parentCategories.Data;
            return View();
        }   
        [HttpPost]
        public async Task<IActionResult> GetCategoryData([FromBody] TermsPagingRequest search)
        {
            if (search == null)
            {
                return BadRequest(new { success = false, message = "Invalid request: search parameter is null" });
            }
            var categories = await _termsService.GetCategoriesPaged(search);
            
            return Json(categories.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(CategoryRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _categoryService.AddCategoryAsync(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(ulong category)
        {
            var AllTag = await _categoryService.GetAllCategoryAsync();
            var SelectedCategory = AllTag.Data.FirstOrDefault(x => x.TermId == category);
            var parentCategories = await _termsService.GetParentCategories();
            ViewBag.ParentCategories = parentCategories.Data;
            var mapped = _mapper.Map<CategoryResponseDto>(SelectedCategory);
            return View(mapped);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryResponseDto model)
        {
            if (!ModelState.IsValid)
            {
                // Fetch Parent Categories again if validation fails
                var parentCategories = await _termsService.GetParentCategories();
                ViewBag.ParentCategories = parentCategories.Data;
                return View(model);
            }

            var response = await _categoryService.UpdateCategoryAsync(model);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory( List<ulong> CategoryIds)
        {
            var result = await _termsService.DeleteTerm(CategoryIds);
            return Json(result);
        }


    }
}
