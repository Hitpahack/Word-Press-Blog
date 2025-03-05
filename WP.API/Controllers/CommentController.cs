using Microsoft.AspNetCore.Mvc;
using WP.DTOs;
using WP.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }
            
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(ulong id)
        {
            var comment = await _commentService.GetCommentById(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newComment = await _commentService.AddComment(createDto);
            return CreatedAtAction(nameof(GetCommentById), new { id = newComment.Data.CommentId }, newComment);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment(ulong id, [FromBody] UpdateCommentDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _commentService.UpdateComment(id, updateDto);
            if (updated ==null) return NotFound();

            return NoContent();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteComment(List<ulong> Ids)
        {
            var deleted = await _commentService.DeleteComment(Ids);
            if (deleted == null) return NotFound();

            return NoContent();
        }

        [HttpPost("approve-comment")]
        public async Task<IActionResult> Approve(ulong commentId)
        {
            var success = await _commentService.ApproveCommentAsync(commentId);
            if (success !=null)
                return Ok(new { message = "Comment approved successfully!" });
            return BadRequest(new { message = "Failed to approve comment." });
        }

        [HttpPost("unapprove-comment")]
        public async Task<IActionResult> Disapprove(ulong commentId)
        {
            var success = await _commentService.DisapproveCommentAsync(commentId);
            if (success !=null)
                return Ok(new { message = "Comment disapproved successfully!" });
            return BadRequest(new { message = "Failed to disapprove comment." });
        }

        [HttpPost("spam-comment")]
        public async Task<IActionResult> MarkSpam(ulong commentId)
        {
            var success = await _commentService.MarkAsSpamAsync(commentId);
            if (success !=null)
                return Ok(new { message = "Comment marked as spam!" });
            return BadRequest(new { message = "Failed to mark comment as spam." });
        }

        [HttpPost("unspam-comment")]
        public async Task<IActionResult> Unspam(ulong commentId)
        {
            var success = await _commentService.UnspamCommentAsync(commentId);
            if (success!=null)
                return Ok(new { message = "Comment restored successfully!" });
            return BadRequest(new { message = "Failed to restore comment." });
        }

        [HttpPost("reply")]
        public async Task<IActionResult> ReplyToComment([FromBody] ReplyRequest request)
        {
            var success = await _commentService.ReplyToCommentAsync(request);
            if (success != null)
                return Ok(new { message = "Reply posted successfully!" });
            return BadRequest(new { message = "Failed to post reply." });
        }




    }
}
