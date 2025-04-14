using BookStore.Application.Helpers;
using BookStore.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BookStore.Api.Helpers
{
    public class JwtHelper: IJwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;

        public JwtHelper(IConfiguration configuration, IDistributedCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public string GenerateJwtToken(int userId, int? role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateAndCacheRefreshTokenAsync(int userId)
        {
            var refreshToken = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddDays(7);

            var tokenData = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                ExpiryDate = expiry
            };

            var jsonData = JsonSerializer.Serialize(tokenData);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiry
            };

            await _cache.SetStringAsync($"refresh_{refreshToken}", jsonData, options);

            return refreshToken;
        }

        public async Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var jsonData = await _cache.GetStringAsync($"refresh_{refreshToken}");
            if (string.IsNullOrEmpty(jsonData)) return null;

            var token = JsonSerializer.Deserialize<RefreshToken>(jsonData);
            if (token == null || token.ExpiryDate < DateTime.UtcNow) return null;

            return token;
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            await _cache.RemoveAsync($"refresh_{refreshToken}");
        }
    }
}
