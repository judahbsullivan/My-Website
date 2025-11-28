using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MyPortfolio.Api.Services
{
    public class TokenService
    {
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _expireMinutes;

        public TokenService(IConfiguration config)
        {
            _jwtKey =
                Environment.GetEnvironmentVariable("JWT_KEY")
                ?? throw new Exception("JWT_KEY missing");
            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "MyPortfolio";
            _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "MyPortfolioUsers";
            _expireMinutes = double.Parse(
                Environment.GetEnvironmentVariable("JWT_EXPIRE_MINUTES") ?? "60"
            );
        }

        public string CreateToken(string userId, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
