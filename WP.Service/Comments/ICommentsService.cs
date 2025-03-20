using AutoMapper;
using jQueryDatatable;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Security.Claims;
using WP.Common;
using WP.DataContext;
using WP.EDTOs;
using WP.EDTOs.Comments;
using WP.EDTOs.Commments;
using WP.EDTOs.Post;
using WP.Repository;
using WP.Service.Categories;
using WP.Service.Users;

namespace WP.Service
{
    public interface ICommentsService : IDisposable
    {
        Task<ResponseDto<Datatable<COMMENT_SP_RESPONSE>>> GetcCommentPaged(CommentPagingRequest reqDto);
        Task<ResponseDto<List<FilterDto>>> GetFiltersAsync();
        Task<ResponseDto<bool>> UpdateCommentStatus(UpdateCommentStatusRequest request);
        Task<ResponseDto<EditCommentDto>> GetEditCommentById(ulong id);
        Task<ResponseDto<bool>> UpdateComment(EditCommentDto reqDto);
        Task<ResponseDto<WpCommentDto>> ReplyComment(ReplyCommentDto replyDto);
    }
    public class CommentsService : BaseServices, ICommentsService
    {
        #region private
        private readonly IRepository<WpComment> _repoComment;
        private readonly IRepository<WpUser> _repoUser;
        private readonly ITermsService _termsService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region ctor
        public CommentsService(IRepository<WpComment> repoComment, IRepository<WpUser> repoUser, ITermsService termsService,
            IMapper mapper, IHttpContextAccessor httpContext, IUsersService usersService)
        {
            _repoComment = repoComment;
            _termsService = termsService;
            _mapper = mapper;
            _httpContext = httpContext;
            _repoUser = repoUser;
        }
        #endregion

        #region functions
        public async Task<ResponseDto<Datatable<COMMENT_SP_RESPONSE>>> GetcCommentPaged(CommentPagingRequest reqDto)
        {
            try
            {
                var query = "CALL GET_COMMENTS_PAGED(@page, @pageSize, @searchText,@CommmentFilter)";

                var jsonsResult = _repoComment.Db.Database.SqlQueryRaw<COMMENT_SP_RESPONSE>(
                    query,
                    new MySqlParameter("@page", reqDto.Page),
                    new MySqlParameter("@pageSize", reqDto.PageSize),
                    new MySqlParameter("@searchText", reqDto.SearchText ?? ""),  // Ensure null values are handled
                    new MySqlParameter("@filterType", reqDto.CommmentFilter ?? "")  // Ensure null values are handled
                ).ToList();


                var data = jsonsResult.Select(r => _mapper.Map<COMMENT_SP_RESPONSE>(r));
                var output = new Datatable<COMMENT_SP_RESPONSE>(data, reqDto.Draw, jsonsResult.FirstOrDefault()?.TotalCount ?? 0, jsonsResult.FirstOrDefault()?.TotalCount ?? 0);

                return await Task.FromResult(new SuccessResponseDto<Datatable<COMMENT_SP_RESPONSE>>(output));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<Datatable<COMMENT_SP_RESPONSE>>(ex.GetActualError()));
            }
        }

        public async Task<ResponseDto<List<FilterDto>>> GetFiltersAsync()
        {
            try
            {
                ulong loggedUserId = Convert.ToUInt64(_httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var query = "CALL GET_COMMENT_FILTERS(@logedUserid)";
                var jsonsResult = _repoComment.Db.Database.SqlQueryRaw<FilterDto>(query,
                 new MySqlParameter("@logedUserid", loggedUserId)
                  ).ToList();
                return await Task.FromResult(new SuccessResponseDto<List<FilterDto>>(jsonsResult));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new FailedResponseDto<List<FilterDto>>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<bool>> UpdateCommentStatus(UpdateCommentStatusRequest request)
        {
            try
            {
                foreach (var Id in request.CommentIds)
                {
                    var findItem = await _repoComment.FindAsync(Id);
                    findItem.CommentApproved = request.Status;
                    _repoComment.Update(findItem);
                }
                return new SuccessResponseDto<bool>(true);
            }
            catch (Exception ex)
            {
                return new FailedResponseDto<bool>(ex.GetActualError());
            }
        }
        public async Task<ResponseDto<EditCommentDto>> GetEditCommentById(ulong id)
        {
            var comments = (from comment in _repoComment.Db.WpComments
                            join post in _repoComment.Db.WpPosts on comment.CommentPostId equals post.Id
                            where comment.CommentId == id
                            select new EditCommentDto
                            {
                                CommentAuthor = comment.CommentAuthor,
                                CommentAuthorEmail = comment.CommentAuthorEmail,
                                CommentAuthorUrl = comment.CommentAuthorUrl,
                                CommentContent = comment.CommentContent,
                                CommentId = comment.CommentId,
                                CommentApproved = comment.CommentApproved,
                                Post_Title = post.PostTitle
                            }).FirstOrDefault();
            if (comments == null)
                return await Task.FromResult(new FailedResponseDto<EditCommentDto>("Data Not found"));

            return await Task.FromResult(new SuccessResponseDto<EditCommentDto>(comments, "Data retrived Successfully"));
        }

        public async Task<ResponseDto<bool>> UpdateComment(EditCommentDto reqDto)
        {
            try
            {
                var comment = await _repoComment.FindAsync(reqDto.CommentId);
                if (comment == null)
                {
                    return new FailedResponseDto<bool>("Comment not found.");
                }
                _mapper.Map(reqDto, comment);
                _repoComment.Update(comment);

                return new SuccessResponseDto<bool>(true);
            }
            catch (Exception ex)
            {
                return new FailedResponseDto<bool>(ex.GetActualError());
            }
        }

        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _repoComment.Dispose();
            _termsService.Dispose();
        }

        public async Task<ResponseDto<WpCommentDto>> ReplyComment(ReplyCommentDto replyDto)
        {
            try
            {
                var parentComment = await _repoComment.FindAsync(replyDto.CommentId);
                if (parentComment == null)
                {
                    return await Task.FromResult(new FailedResponseDto<WpCommentDto>("Data Not found)"));
                }
                ulong loggedUserId = Convert.ToUInt64(_httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _repoUser.FindAsync(loggedUserId);
                WpComment comment = new WpComment();
                comment.CommentParent = replyDto.CommentId;
                comment.CommentContent = replyDto.ReplyContent;
                comment.CommentAuthor = user.UserLogin;
                comment.CommentDateGmt = DateTime.UtcNow;
                comment.CommentDate = DateTime.Now;
                comment.CommentApproved = "approved";
                comment.CommentPostId = parentComment.CommentPostId;
                comment.CommentAuthorEmail = user.UserEmail;
                comment.UserId = loggedUserId;

                var insertedCommentEntry = await _repoComment.InsertAsync(comment);
                var insertedComment = insertedCommentEntry.Entity; // Get the actual inserted entity
                var result = _mapper.Map<WpCommentDto>(insertedComment);

                return await Task.FromResult(new SuccessResponseDto<WpCommentDto>(result));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new FailedResponseDto<WpCommentDto>(ex.GetActualError()));
            }



        }
    }

}
