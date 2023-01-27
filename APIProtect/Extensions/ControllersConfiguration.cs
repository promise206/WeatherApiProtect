using FluentValidation.AspNetCore;

namespace APIProtect.Extensions
{
    public static class ControllersConfiguration
    {
        public static void AddControllersExtension(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        }
    }
}
