using Microsoft.AspNetCore.Mvc;
using WP.Service.Users;
using WP.Services;


namespace WP.Web.Controllers.Components
{
    public class UsersFilterComponent : ViewComponent
    {
        private readonly IUsersService _usersService;
        public UsersFilterComponent(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filters = await _usersService.GetFiltersAsync();
            return View(filters);
        }
    }
}
