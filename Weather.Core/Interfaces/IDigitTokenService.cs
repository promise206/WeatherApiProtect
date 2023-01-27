using Microsoft.AspNetCore.Identity;
using Weather.Domain.Entities;

namespace Weather.Core.Interfaces
{
    public interface IDigitTokenService
    {
        Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user);
        Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user);
        Task<string> GetUserModifierAsync(string purpose, UserManager<User> manager, User user);
        Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user);
    }
}