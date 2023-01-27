using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Enums;
using Weather.Domain.Entities;

namespace Weather.Infrastructure
{
    public class WeatherDbInitializer
    {
        public static async Task Seed(IApplicationBuilder builder)
        {
            using var serviceScope = builder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<WeatherDbContext>();
            string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, @"Weather.Infrastructure\Data\");
            if (await context.Database.EnsureCreatedAsync()) return;

            if (!context.Roles.Any())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var readText = await File.ReadAllTextAsync(filePath + "Roles.json");
                List<IdentityRole> Roles = JsonConvert.DeserializeObject<List<IdentityRole>>(readText);
                foreach (var role in Roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
            if (!context.AppUser.Any())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var readText = await File.ReadAllTextAsync(filePath + "Users.json");
                List<User> users = JsonConvert.DeserializeObject<List<User>>(readText);
                users.ForEach(delegate (User user) {
                    userManager.CreateAsync(user, "Jaspino2_06$");
                    userManager.AddToRoleAsync(user, UserRole.Visitor.ToString());
                    context.AppUser.AddAsync(user);
                });
            }
            await context.SaveChangesAsync();
        }
    }
}
