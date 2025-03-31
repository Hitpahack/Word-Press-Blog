using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.EDTOs.Comments;
using WP.EDTOs.Commments;


namespace WP.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly Service.ICommentsService _commentService;
        private readonly ILogger<CommentsController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public CommentsController(Service.ICommentsService commentService, ILogger<CommentsController> logger, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _commentService = commentService;
            _logger = logger;
            _mapper = mapper;
            _httpContext = httpContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetCommentsData([FromBody] CommentPagingRequest search)
        {
            var result = await _commentService.GetcCommentPaged(search);
            return Json(result.Data);
        }
        [HttpPost("UpdateCommentStatus")]
        public async Task<IActionResult> UpdateCommentStatus([FromBody] UpdateCommentStatusRequest request)
        {

            if (request == null || request.CommentIds == null || !request.CommentIds.Any())
            {
                return BadRequest("Invalid request");
            }
            var result = await _commentService.UpdateCommentStatus(request);
            return Json(result);

        }
        public async Task<IActionResult> EditComment(ulong comment)
        {
            var result = await _commentService.GetEditCommentById(comment);
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(ulong comment,EditCommentDto model )
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _commentService.UpdateComment(model);
            if (!result.Success)
            {
                _logger.LogError(result.Message);
                return BadRequest(result);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ReplyComment([FromBody] ReplyCommentDto replyDto)
        {
            if (string.IsNullOrEmpty(replyDto.ReplyContent))
            {
                return BadRequest("Reply content cannot be empty.");
            }

            var response = await _commentService.ReplyComment(replyDto);
            return Json(response);
        }

    }
}
