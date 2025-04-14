using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Application.Helpers
{
    public interface IJwtHelper
    {
        string GenerateJwtToken(int userId, int? role);

        Task<string> GenerateAndCacheRefreshTokenAsync(int userId);

        Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken);

        Task InvalidateRefreshTokenAsync(string refreshToken);
    }
}

