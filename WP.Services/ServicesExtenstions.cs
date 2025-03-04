using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Core;
using WP.Data.Repositories;

namespace WP.Services
{
    public static class ServicesExtenstions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection service, Action<IServiceCollection> callback = null)
        {
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<IPostRepository, PostRepository>();
            service.AddScoped<ITagRepository, TagRepository>();
            service.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();
            service.AddScoped<IPageRepository, PageRepository>();
            service.AddScoped<IPageService, PageService>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IPostService, PostService>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<ITagService, TagService>();
            service.AddScoped<ITokenService, TokenService>();
            if (callback != null)
            {
                callback.Invoke(service);
            }
            return service;
        }
    }
}
