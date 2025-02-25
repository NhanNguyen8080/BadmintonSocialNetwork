using BadmintonSocialNetwork.Repository.Interfaces;
using BadmintonSocialNetwork.Repository.Models;
using BadmintonSocialNetwork.Service.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.Services
{
    public interface IJwtTokenFactory
    {
        Task<AuthenticationResult> CreateTokenAsync(Account account);
        void RevokeRefreshToken(string refreshToken);
        bool ValidateRefreshToken(string refreshToken, out string? username);
        ClaimsPrincipal? ValidateToken(string token);
    }
    public class JwtTokenFactory : IJwtTokenFactory
    {
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;
        private readonly string _secretKey;
        private static readonly Dictionary<string, string> RefreshTokens = new();

        public JwtTokenFactory(IConfiguration config, IAccountService accountService)
        {
            _config = config;
            _accountService = accountService;
            DotNetEnv.Env.Load();
            _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new ArgumentNullException("JWT_SECRET environment variable is not set.");
        }

        public async Task<AuthenticationResult> CreateTokenAsync(Account account)
        {
            var roles = await _accountService.GetUserRoles(account.Id);

            var claims = new List<Claim>
            {
                new Claim("UserName", account.UserName),
                new Claim("Email", account.Email),
                new Claim("Id", account.Id.ToString()),
                new Claim("PhoneNumber", account.PhoneNumber),
            }; 
            
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config.GetSection("AppSettings:Issuer")?.Value,
                audience: _config.GetSection("AppSettings:Audience")?.Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = Guid.NewGuid().ToString();
            RefreshTokens[refreshToken] = account.UserName;

            var authenticationResult = new AuthenticationResult
            {
                Token = tokenHandler,
                RefreshToken = refreshToken,
                Expires = DateTime.Now.AddMinutes(30),
                Username = account.UserName,
                Email = account.Email,
                Roles = roles,
            };
            return authenticationResult;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateRefreshToken(string refreshToken, out string? username)
        {
            return RefreshTokens.TryGetValue(refreshToken, out username);
        }

        public void RevokeRefreshToken(string refreshToken)
        {
            RefreshTokens.Remove(refreshToken);
        }
    }
}
