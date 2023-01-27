using FluentValidation;
using Weather.Core.DTOs;
using Weather.Core.Interfaces;
using Weather.Core.Services;
using Weather.Core.Utility;

namespace APIProtect.Extensions
{
    public static class RegisterServicesConfiguration
    {
        public static void AddRegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDigitTokenService, DigitTokenService>();
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAuthenticationExtension(configuration);
            services.AddAuthorizationExtension();
            services.AddTransient<IValidator<LoginDTO>, LoginUserValidator>();
            services.AddTransient<IValidator<RegistrationDTO>, UserRegistrationValidator>();
        }
    }
}
