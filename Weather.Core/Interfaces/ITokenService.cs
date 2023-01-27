using Weather.Domain.Entities;

namespace Weather.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        Task<string> GenerateToken(User user);
    }
}