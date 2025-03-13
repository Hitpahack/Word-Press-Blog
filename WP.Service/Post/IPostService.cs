using AutoMapper;
using jQueryDatatable;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Security.Claims;
using WP.Common;
using WP.DataContext;
using WP.EDTOs;
using WP.EDTOs.Post;
using WP.Repository;
using WP.Service.Categories;

namespace WP.Service
{
    public interface IPostService : IDisposable
    {
        Task<ResponseDto<Datatable<POST_DT_RESPONSE>>> GetPostPaged(PostPagingRequest reqDto);
        Task<ResponseDto<Datatable<PAGE_DT_RESPONSE>>> GetPagesPaged(PostPagingRequest reqDto);
        Task<ResponseDto<POST_DTO>> GetPost(ulong postid);
        Task<ResponseDto<POST_DTO>> GetPage(ulong postid);
        Task<ResponseDto<POST_DTO>> AddUpdatePost(WP_POST_ADD_DTO reqDto, ulong postid = 0);
        Task<ResponseDto<POST_DTO>> AddUpdatePage(WP_PAGE_ADD_DTO reqDto, ulong postid = 0);
        Task<ResponseDto<bool>> DeletePost(ulong postid);
        Task<ResponseDto<bool>> DeletePost(ulong[] postid);
        Task<List<FilterDto>> GetPostFiltersAsync();
        Task<List<FilterDto>> GetPageFiltersAsync();
    }
    public class PostService : BaseServices, IPostService
    {
        #region private
        private readonly IRepository<WpPost> _repoPost;
        private readonly ITermsService _termsService;
        //private readonly IRepository<GET_POSTS_PAGED_SP> _get_posts_paged_sp;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region ctor
        public PostService(IRepository<WpPost> repoPost, ITermsService termsService,
            //IRepository<GET_POSTS_PAGED_SP> get_posts_paged_sp, 
            IMapper mapper, IHttpContextAccessor httpContext )
        {
            //_get_posts_paged_sp = get_posts_paged_sp;
            _repoPost = repoPost;
            _termsService = termsService;
            _mapper = mapper;
            _httpContext = httpContext;
        }
        #endregion

        #region functions
        public async Task<ResponseDto<Datatable<POST_DT_RESPONSE>>> GetPostPaged(PostPagingRequest reqDto)
        {
            try
            {
                var query = "CALL GET_POSTS_PAGED(@page, @pageSize, @searchText, @status, @PostDate, @CategoryId, @RankMathFilter)";

                var jsonsResult = _repoPost.Db.Database.SqlQueryRaw<POST_SP_RESPONSE>(
                    query,
                    new MySqlParameter("@page", reqDto.Page),
                    new MySqlParameter("@pageSize", reqDto.PageSize),
                    new MySqlParameter("@searchText", reqDto.SearchText ?? ""),  // Ensure null values are handled
                    new MySqlParameter("@status", reqDto.Status ?? ""),  // Ensure null values are handled
                    new MySqlParameter("@PostDate", reqDto.Date),  // Ensure null values are handled
                    new MySqlParameter("@CategoryId", reqDto.CategoryId),
                    new MySqlParameter("@RankMathFilter", reqDto.RankMathFilter ?? "")  // Ensure null values are handled
                ).ToList();


                var data = jsonsResult.Select(r => _mapper.Map<POST_DT_RESPONSE>(r));
                var output = new Datatable<POST_DT_RESPONSE>(data, reqDto.Draw, jsonsResult.FirstOrDefault()?.TotalCount ?? 0, jsonsResult.FirstOrDefault()?.TotalCount ?? 0);

                return await Task.FromResult(new SuccessResponseDto<Datatable<POST_DT_RESPONSE>>(output));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<Datatable<POST_DT_RESPONSE>>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<Datatable<PAGE_DT_RESPONSE>>> GetPagesPaged(PostPagingRequest reqDto)
        {
            try
            {
                var query = "CALL GET_PAGES_PAGED(@page, @pageSize, @searchText, @Date, @RankMathFilter)";

                var jsonsResult = _repoPost.Db.Database.SqlQueryRaw<PAGE_SP_RESPONSE>(
                    query,
                    new MySqlParameter("@page", reqDto.Page),
                    new MySqlParameter("@pageSize", reqDto.PageSize),
                    new MySqlParameter("@searchText", reqDto.SearchText ?? ""),  // Ensure null values are handled
                    new MySqlParameter("@Date", reqDto.Date),  // Ensure null values are handled
                    new MySqlParameter("@RankMathFilter", reqDto.RankMathFilter ?? "")  // Ensure null values are handled
                ).ToList();


                var data = jsonsResult.Select(r => _mapper.Map<PAGE_DT_RESPONSE>(r));
                var output = new Datatable<PAGE_DT_RESPONSE>(data, reqDto.Draw, jsonsResult.FirstOrDefault()?.TotalCount ?? 0, jsonsResult.FirstOrDefault()?.TotalCount ?? 0);

                return await Task.FromResult(new SuccessResponseDto<Datatable<PAGE_DT_RESPONSE>>(output));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<Datatable<PAGE_DT_RESPONSE>>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<POST_DTO>> GetPost(ulong postid)
        {
            try
            {
                var query = "CALL GET_POST_BYID(@postid)";

                var post = _repoPost.SqlQueryRawSingle<POST_DTO>(query, new MySqlParameter("@postid", postid));
                

                return await Task.FromResult(new SuccessResponseDto<POST_DTO>(post));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<POST_DTO>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<POST_DTO>> GetPage(ulong postid)
        {
            try
            {
                var query = "CALL GET_PAGE_BYID(@postid)";

                var post = _repoPost.SqlQueryRawSingle<POST_DTO>(query, new MySqlParameter("@postid", postid));
                

                return await Task.FromResult(new SuccessResponseDto<POST_DTO>(post));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<POST_DTO>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<POST_DTO>> AddUpdatePost(WP_POST_ADD_DTO reqDto, ulong postid = 0)
        {
            try
            {

                WpPost wppost;
                if (postid > 0)
                {
                    reqDto.Id = postid;
                    wppost = _repoPost.GetFirstOrDefault(s => s.Id == postid);
                    //_mapper.Map(reqDto, wppost);
                    wppost.PostModified = DateTime.Now;
                    wppost.PostModifiedGmt = DateTime.UtcNow;
                    wppost.PostTitle = reqDto.Post_Title;
                    wppost.PostName = reqDto.Post_Name;
                    _repoPost.Update(wppost);
                }
                else
                {
                    wppost = _mapper.Map<WpPost>(reqDto);
                    wppost.PostDate = DateTime.Now;
                    wppost.PostDateGmt = DateTime.UtcNow;
                    wppost.PostType = "post";
                    await _repoPost.InsertAsync(wppost);
                }

                await _termsService.AssignRemoved_Category_To_Post(wppost.Id, reqDto.Categories.ToArray());
                await _termsService.AssignRemoved_Tag_To_Post(wppost.Id, reqDto.Tags.ToArray());
                POST_DTO postdto = _mapper.Map<POST_DTO>(wppost);
                return await Task.FromResult(new SuccessResponseDto<POST_DTO>(postdto));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<POST_DTO>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<POST_DTO>> AddUpdatePage(WP_PAGE_ADD_DTO reqDto, ulong postid = 0)

        {
            try
            {

                WpPost wppost;
                if (postid > 0)
                {
                    reqDto.Id = postid;
                    wppost = _repoPost.GetFirstOrDefault(s => s.Id == postid);
                    //_mapper.Map(reqDto, wppost);
                    wppost.PostModified = DateTime.Now;
                    wppost.PostModifiedGmt = DateTime.UtcNow;
                    wppost.PostTitle = reqDto.Post_Title;
                    wppost.PostName = reqDto.Post_Name;
                    wppost.PostContent = reqDto.Post_Content;
                    _repoPost.Update(wppost);
                }
                else
                {
                    wppost = _mapper.Map<WpPost>(reqDto);
                    wppost.PostDate = DateTime.Now;
                    wppost.PostDateGmt = DateTime.UtcNow;
                    wppost.PostType = "page";
                    await _repoPost.InsertAsync(wppost);
                }

                //await _termsService.AssignRemoved_Category_To_Post(wppost.Id, reqDto.Categories.ToArray());
                //await _termsService.AssignRemoved_Tag_To_Post(wppost.Id, reqDto.Tags.ToArray());
                POST_DTO postdto = _mapper.Map<POST_DTO>(wppost);
                return await Task.FromResult(new SuccessResponseDto<POST_DTO>(postdto));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<POST_DTO>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<bool>> DeletePost(ulong postid)
        {
            var findItem = await _repoPost.FindAsync(postid);
            findItem.PostStatus = "trash";
            _repoPost.Update(findItem);
            return new SuccessResponseDto<bool>(true);
        }
        public async Task<ResponseDto<bool>> DeletePost(ulong[] postid)
        {
            foreach (var item in postid)
            {
                var findItem = await _repoPost.FindAsync(item);
                findItem.PostStatus = "trash";
                _repoPost.Update(findItem);
            }
            return new SuccessResponseDto<bool>(false);
        }
        public async Task<List<FilterDto>> GetPostFiltersAsync()
        {
            try
            {
                ulong loggedUserId = Convert.ToUInt64(_httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var query = "CALL GET_POST_FILTERS(@logedUserid)";
                var jsonsResult = _repoPost.Db.Database.SqlQueryRaw<FilterDto>(query,
                    new MySqlParameter("@logedUserid", loggedUserId)
                ).ToList();
                return await Task.FromResult(jsonsResult);
            }
            catch (Exception ex)
            {
                return new List<FilterDto>(); // Return an empty list on failure
            }
        }

        public async Task<List<FilterDto>> GetPageFiltersAsync()
        {
            try
            {
                ulong loggedUserId = Convert.ToUInt64(_httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var query = "CALL GET_PAGE_FILTERS(@logedUserid)";
                var jsonsResult = _repoPost.Db.Database.SqlQueryRaw<FilterDto>(query,
                    new MySqlParameter("@logedUserid", loggedUserId)
                ).ToList();
                return await Task.FromResult(jsonsResult);
            }
            catch (Exception ex)
            {
                return new List<FilterDto>(); // Return an empty list on failure
            }
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _repoPost.Dispose();
            _termsService.Dispose();
        }

        
    }

}
