using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.API.Controllers;
using WP.EDTOs.Comments;
using WP.EDTOs.Commments;


namespace WP.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly Service.ICommentsService _commentService;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public CommentsController(Service.ICommentsService commentService, ILogger<UserController> logger, IMapper mapper, IHttpContextAccessor httpContext)
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
            if (result.Data)
                return Ok(new { message = "Comments updated successfully" });
            else
                return StatusCode(500, "Failed to update comments");

        }
    }
}
