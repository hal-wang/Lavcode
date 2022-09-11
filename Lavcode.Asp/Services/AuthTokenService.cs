using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Lavcode.Asp.Services
{
    public class AuthTokenService
    {
        private readonly JwtBearerOptions _jwtBearerOptions;
        private readonly SigningCredentials _signingCredentials;

        public AuthTokenService(
           IOptionsSnapshot<JwtBearerOptions> jwtBearerOptions,
            SigningCredentials signingCredentials)
        {
            _jwtBearerOptions = jwtBearerOptions.Get(JwtBearerDefaults.AuthenticationScheme);
            _signingCredentials = signingCredentials;
        }

        public string CreateJwtToken()
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("from", "lavcode"),
                }),
                Issuer = _jwtBearerOptions.TokenValidationParameters.ValidIssuer,
                Audience = _jwtBearerOptions.TokenValidationParameters.ValidAudience,
                Expires = DateTime.UtcNow.AddDays(90),
                SigningCredentials = _signingCredentials
            };

            var handler = _jwtBearerOptions.SecurityTokenValidators.OfType<JwtSecurityTokenHandler>().FirstOrDefault()
                ?? new JwtSecurityTokenHandler();
            var securityToken = handler.CreateJwtSecurityToken(tokenDescriptor);
            var token = handler.WriteToken(securityToken);

            return token;
        }
    }

}
