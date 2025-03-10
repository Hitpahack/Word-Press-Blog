
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WP.Common;
using WP.Services;

namespace WP.Service
{
    public class BaseServices
    {
        public static string[] DefaultRoles => AppExtenstions.Roles;
        public readonly AppSettings _appSetting;
        public readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
        public readonly IConfiguration _config;
        public readonly IMemoryCache _cache;
        public readonly string _connectionstring;
        public BaseServices()
        {
            using (var serviceScope = ServiceActivator.GetScope())
            {
                _config = serviceScope.ServiceProvider.GetService<IConfiguration>();
                _connectionstring = _config.GetConnectionString("DefaultConnection");
                _appSetting = serviceScope.ServiceProvider.GetService<AppSettings>();
                _environment = serviceScope.ServiceProvider.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
                _cache = serviceScope.ServiceProvider.GetService<IMemoryCache>();
            }
        }
        
    }
}
