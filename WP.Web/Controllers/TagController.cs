using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.DTOs;
using WP.EDTOs.Categories;
using WP.Service.Categories;
using WP.Services;


namespace WP.Web.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;
        private readonly ITermsService _termsService;
        public TagController(ITagService tagService, IMapper mapper, ITermsService termsService)
        {
            _tagService = tagService;
            _mapper = mapper;
            _termsService = termsService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetTagData([FromBody] TermsPagingRequest search)
        {
            if (search == null)
            {
                return BadRequest(new { success = false, message = "Invalid request: search parameter is null" });
            }
            var tags = await _termsService.GetTagsPaged(search);
            return Json(tags.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTag(TagRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _tagService.AddTagAsync(model);

            if (!result.Success)
            {
                return View(model);
            }
            if (model.AsJson==true)
                return Json(result);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditTag(ulong tag)
        {
            var AllTag = await _tagService.GetAllTagAsync();
            var SelectedTag = AllTag.Data.FirstOrDefault(x => x.TermId == tag);
            var mapped = _mapper.Map<UpdateTagDto>(SelectedTag);
            return View(mapped);
        }

        [HttpPost]
        public async Task<IActionResult> EditTag(UpdateTagDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _tagService.UpdateTagAsync(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTag(List<ulong> TagIds)
        {
            
            var result = await _termsService.DeleteTerm(TagIds);

            if (result.Success)
            {
                return Json(new { success = true, message = "Tag deleted successfully!" });
            }

            return Json(new { success = false, message = "Failed to delete tag." });
        }




    }
}
