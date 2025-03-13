using AutoMapper;
using jQueryDatatable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MySqlConnector;
using WP.Common;
using WP.DataContext;
using WP.EDTOs;
using WP.EDTOs.Post;
using WP.EDTOs.Users;
using WP.Repository;
using WP.Service.Categories;

namespace WP.Service.Users
{
    public interface IUsersService : IDisposable
    {
        Task<ResponseDto<Datatable<USERS_DT_RESPONSE>>> GetUsersPaged(UsersPagingRequest reqDto);
        Task<ResponseDto<List<FilterDto>>> GetFiltersAsync();
        Task<ResponseDto<bool>> DeleteUsers(string userIds, string actionType, ulong? newOwnerId = null);
    }
    public class UsersService : BaseServices, IUsersService
    {
        #region private
        private readonly IRepository<WpUser> _repoUsers;
        private readonly ITermsService _termsService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpcontext;
        #endregion

        #region ctor
        public UsersService(IRepository<WpUser> repoUsers, ITermsService termsService, IMapper mapper,IHttpContextAccessor httpcontext)
        {
            _repoUsers = repoUsers;
            _termsService = termsService;
            _mapper = mapper;
            _httpcontext = httpcontext;
        }
        #endregion

        #region functions
        public async Task<ResponseDto<Datatable<USERS_DT_RESPONSE>>> GetUsersPaged(UsersPagingRequest reqDto)
        {
            try
            {
                var query = "CALL GET_USERS_PAGED(@page, @pageSize, @searchText, @status, @date)";
                var jsonsResult = _repoUsers.Db.Database.SqlQueryRaw<USERS_SP_RESPONSE>(
                    query,
                    new MySqlParameter("@page", reqDto.Page),
                    new MySqlParameter("@pageSize", reqDto.PageSize),
                    new MySqlParameter("@searchText", reqDto.SearchText ?? ""),  
                    new MySqlParameter("@status", reqDto.Status ?? ""),  
                    new MySqlParameter("@date", reqDto.FDate) 
                ).ToList();


                var data = jsonsResult.Select(r => _mapper.Map<USERS_DT_RESPONSE>(r));
                var output = new Datatable<USERS_DT_RESPONSE>(data, reqDto.Draw, jsonsResult.FirstOrDefault()?.TotalCount ?? 0, jsonsResult.FirstOrDefault()?.TotalCount ?? 0);

                return await Task.FromResult(new SuccessResponseDto<Datatable<USERS_DT_RESPONSE>>(output));
            }
            catch (Exception ex)
            {

                return await Task.FromResult(new FailedResponseDto<Datatable<USERS_DT_RESPONSE>>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<List<FilterDto>>> GetFiltersAsync()
        {
            try
            {
                var query = "CALL GET_USER_FILTERS()";
                var jsonsResult = _repoUsers.Db.Database.SqlQueryRaw<FilterDto>(query).ToList();
                return await Task.FromResult( new SuccessResponseDto<List<FilterDto>>(jsonsResult,"Succesfully Get"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new FailedResponseDto<List<FilterDto>>(ex.GetActualError()));
            }
        }
        public async Task<ResponseDto<bool>> DeleteUsers(string userIds, string actionType, ulong? newOwnerId = null)
        {
            try
            {
                var query = "CALL DELETE_USER(@userIds, @actionType, @newOwnerId)";
               await _repoUsers.Db.Database.ExecuteSqlRawAsync(
                    query,
                   new MySqlParameter("@userIds", userIds),
                    new MySqlParameter("@actionType", actionType),
                    new MySqlParameter("@newOwnerId", newOwnerId.HasValue ? (object)newOwnerId.Value : DBNull.Value)
                );

                return new SuccessResponseDto<bool>(true);
            }
            catch (Exception ex)
            {
                return new FailedResponseDto<bool>("An error occurred: " + ex.Message);
            }
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _repoUsers.Dispose();
        }

        
    }

}
