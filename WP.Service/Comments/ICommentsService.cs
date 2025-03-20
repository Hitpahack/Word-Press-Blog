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
using WP.Service.Medias;

namespace WP.Service
{
    public interface ICommentsService : IDisposable
    {
        Task<ResponseDto<Datatable<COMMENT_SP_RESPONSE>>> GetcCommentPaged(CommentPagingRequest reqDto);
        Task<ResponseDto<List<FilterDto>>> GetFiltersAsync();
        Task<ResponseDto<bool>> UpdateCommentStatus(UpdateCommentStatusRequest request);
    }
    public class CommentsService : BaseServices, ICommentsService
    {
        #region private
        private readonly IRepository<WpComment> _repoComment;
        private readonly ITermsService _termsService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region ctor
        public CommentsService(IRepository<WpComment> repoComment, ITermsService termsService,
            IMapper mapper, IHttpContextAccessor httpContext )
        {
            _repoComment = repoComment;
            _termsService = termsService;
            _mapper = mapper;
            _httpContext = httpContext;
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

        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _repoComment.Dispose();
            _termsService.Dispose();
        }

        
    }

}
