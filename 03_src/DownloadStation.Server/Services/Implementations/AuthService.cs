using DownloadStation.Server.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Implementations
{
    /// <summary>
    /// 取代复杂的 Role/User 机制，此组件仅比对单一配置密码进行身份发放。
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public Task<string?> LoginAsync(string password)
        {
            var adminPassword = _config.GetValue<string>("AppSettings:AdminPassword");
            
            // 安全阻断：若配置密码缺失或者校验未能完全命中目标，默认拒绝通过
            if (string.IsNullOrEmpty(adminPassword) || password != adminPassword)
            {
                return Task.FromResult<string?>(null);
            }

            var secretKey = _config.GetValue<string>("AppSettings:JwtSecret");
            if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
            {
                throw new InvalidOperationException("Fatal: JWT Secret is empty or too short. Please set AppSettings:JwtSecret to at least 32 characters.");
            }

            var expirationStr = _config.GetValue<string>("AppSettings:JwtExpirationHours") ?? "24";
            if (!int.TryParse(expirationStr, out var expirationHours))
            {
                expirationHours = 24;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Name, "Administrator")
                }),
                Expires = DateTime.UtcNow.AddHours(expirationHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult<string?>(tokenHandler.WriteToken(token));
        }
    }
}
