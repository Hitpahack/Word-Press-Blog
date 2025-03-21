using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.DTOs;
using WP.EDTOs.Commments;
using WP.Services;

namespace WP.Web.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;
        public TagController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetTagData()
        {
            var tags = await _tagService.GetAllTagAsync();
            return Json(tags.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AddTag(TagRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _tagService.AddTagAsync(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }
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
        public async Task<IActionResult> DeleteTag(List<ulong> id)
        {
            var result = await _tagService.DeleteTagAsync(id);

            if (result.Success)
            {
                return Json(new { success = true, message = "Tag deleted successfully!" });
            }

            return Json(new { success = false, message = "Failed to delete tag." });
        }




    }
}
