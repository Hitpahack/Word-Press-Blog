using Microsoft.Extensions.DependencyInjection;
using WP.Service.Categories;
using WP.Service.Users;

namespace WP.Service
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddWebService(this IServiceCollection service, Action<IServiceCollection> callback = null)
        {
            service.AddSingleton<IPostService, PostService>();
            service.AddSingleton<IUsersService, UsersService>();
            service.AddSingleton<ITermsService, TermsService>();
            if (callback != null)
            {
                callback.Invoke(service);
                callback.Invoke(service);
            }
            return service;
        }
        
    }
}
