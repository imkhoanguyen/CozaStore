using CozaStore.Data;
using CozaStore.Models;

namespace CozaStore.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
        {
            //identity services
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<DataContext>();

            //cookie config
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "CozaCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                options.SlidingExpiration = true;
            });

            return services;
        }
    }
}
