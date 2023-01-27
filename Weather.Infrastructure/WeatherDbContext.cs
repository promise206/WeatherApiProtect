using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Weather.Domain.Entities;

namespace Weather.Infrastructure
{
    public class WeatherDbContext : IdentityDbContext<User>
    {
        private const string UPDATEDAT = "UpdatedAt";
        private const string CREATEDAT = "CreatedAt";
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        {

        }
        public DbSet<User> AppUser { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is User appUser)
                {
                    AuditPropertiesChange(item.State, appUser);
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public static void AuditPropertiesChange<T>(EntityState state, T obj) where T : class
        {
            PropertyInfo? value;
            switch (state)
            {
                case EntityState.Modified:
                    value = obj.GetType().GetProperty(UPDATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTimeOffset.UtcNow);
                    break;
                case EntityState.Added:
                    value = obj.GetType().GetProperty(CREATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTimeOffset.UtcNow);
                    value = obj.GetType().GetProperty(UPDATEDAT);
                    if (value != null)
                        value.SetValue(obj, DateTimeOffset.UtcNow);
                    break;
                default:
                    break;
            }
        }
    }
}
