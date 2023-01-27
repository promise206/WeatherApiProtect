using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weather.Core.Services;
using Weather.Domain.Entities;
using Weather.Infrastructure;

namespace APIProtect.Extensions
{
    public static class ConnectionConfiguration
    {
        public static void AddDbContextAndConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            services.AddDbContextPool<WeatherDbContext>(options =>
            {
                string connStr;
                connStr = config.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connStr);
            });

            var builder = services.AddIdentity<User, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 8;
                x.Password.RequireDigit = false;
                x.Password.RequireUppercase = true;
                x.Password.RequireLowercase = true;
                x.User.RequireUniqueEmail = true;
                x.SignIn.RequireConfirmedEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            _ = builder.AddEntityFrameworkStores<WeatherDbContext>()
            .AddTokenProvider<DigitTokenService>(DigitTokenService.DIGITEMAIL)
            .AddDefaultTokenProviders();
        }
    }
}
