using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using WP.Core.Middleware;
using WP.Data;
using WP.Services;
using WP.Web.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new GlobalDateTimeConverter());
});
#region appsettings
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("App_Data/appsettings.json", true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = string.Format("App_Data/appsettings.{0}.json", builder.Environment.EnvironmentName);
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();
#endregion

#region logs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/error.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
    .WriteTo.File("Logs/all-logs.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
#endregion

#region dbcontext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlogContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
#endregion

#region Session
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/accounts/login?ReturnUrl=''"; 
        options.LogoutPath = "/accounts/logout";
        options.AccessDeniedPath = "/accounts/access-denied"; 
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

#endregion

#region JWT
if (false) {
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });
    builder.Services.AddAuthorization();
}
#endregion

builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.ConfigureApplicationSettings(builder);

//var appSettings = Singleton<AppSettings>.Instance;
//var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

//if (useAutofac)
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//else
builder.Host.UseDefaultServiceProvider(options =>
    {
        options.ValidateScopes = false;
        options.ValidateOnBuild = true;
    });

//add services to the application and configure service provider
//builder.Services.ConfigureApplicationServices(builder);

//register all settings
//var typeFinder = Singleton<ITypeFinder>.Instance;

//var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
//foreach (var setting in settings)
//{
//    builder.Services.AddScoped(setting, serviceProvider =>
//    {
//        var storeId = DataSettingsManager.IsDatabaseInstalled()
//            ? serviceProvider.GetRequiredService<IStoreContext>().GetCurrentStore()?.Id ?? 0
//            : 0;

//        return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting, storeId).Result;
//    });
//}


//if (!useAutofac)
//    builder.Services.AddScoped(typeof(Lazy<>), typeof(LazyInstance<>));

//admin factories
//builder.Services.AddFluentMigratorCore();
builder.Services.AddWebServices();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
//builder.Services.AddScoped<IBaseAdminModelFactory, BaseAdminModelFactory>();
//builder.Services.AddScoped<ICommonModelFactory, CommonModelFactory>();
//builder.Services.AddScoped<IPluginModelFactory, PluginModelFactory>();
//builder.Services.AddScoped<IPluginService, PluginService>();
//builder.Services.AddScoped<ISearchPluginManager, SearchPluginManager>();
//builder.Services.AddScoped<IUploadService, UploadService>();
//builder.Services.AddScoped<IWidgetPluginManager, WidgetPluginManager>();
//builder.Services.AddScoped<ICustomerService, CustomerService>();
//builder.Services.AddSingleton<IMigrationManager, MigrationManager>();
//builder.Services.AddScoped<IScheduleTaskService, ScheduleTaskService>();
//builder.Services.AddSingleton<ITaskScheduler, Nop.Services.ScheduleTasks.TaskScheduler>();
//builder.Services.AddTransient<IScheduleTaskRunner, ScheduleTaskRunner>();
//builder.Services.AddScoped<ISettingService, SettingService>();
//builder.Services.AddSingleton<IEventPublisher, EventPublisher>();
//builder.Services.AddScoped<INopFileProvider, NopFileProvider>();
//builder.Services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
//builder.Services.AddScoped<IWebHelper, WebHelper>();
//builder.Services.AddScoped<IShortTermCacheManager, PerRequestCacheManager>();
//builder.Services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
//builder.Services.AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>));
//builder.Services.AddScoped<IStoreContext, WebStoreContext>();
//builder.Services.AddScoped<OfficialFeedManager>();
//builder.Services.AddScoped<INopHtmlHelper, NopHtmlHelper>();
//builder.Services.AddScoped<INopAssetHelper, NopAssetHelper>();
//builder.Services.AddScoped<IPermissionService, PermissionService>();
//builder.Services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
var app = builder.Build();
#region middelware
app.UseMiddleware<ExceptionMiddleware>();
//app.UseMiddleware<JwtSessionMiddleware>();
#endregion
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Category}/{action=EditCategory}/{id?}");

app.Run();
