using AutoMapper;
using jQueryDatatable;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using WP.Common;
using WP.DataContext;
using WP.EDTOs;
using WP.EDTOs.Users;
using WP.Repository;
using WP.Service.Categories;

namespace WP.Service.Users
{
    public interface IUsersService : IDisposable
    {
        Task<ResponseDto<Datatable<USERS_DT_RESPONSE>>> GetUsersPaged(UsersPagingRequest reqDto);

    }
    public class UsersService : BaseServices, IUsersService
    {
        #region private
        private readonly IRepository<WpUser> _repoUsers;
        private readonly ITermsService _termsService;
        private readonly IMapper _mapper;
        #endregion

        #region ctor
        public UsersService(IRepository<WpUser> repoUsers, ITermsService termsService, IMapper mapper)
        {
            _repoUsers = repoUsers;
            _termsService = termsService;
            _mapper = mapper;
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
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _repoUsers.Dispose();
        }

    }

}
