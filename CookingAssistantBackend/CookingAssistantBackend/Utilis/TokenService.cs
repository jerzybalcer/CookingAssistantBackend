using Backend.Services;
using CookingAssistantBackend.Models;
using CookingAssistantBackend.Models.Database;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CookingAssistantBackend.Utilis
{
        public class TokenService : ITokenService
        {
            private readonly IConfiguration _configuration;
            private readonly CookingAssistantContext _Context;

            public TokenService(IConfiguration configuration, CookingAssistantContext context)
            {
                _configuration = configuration;
                _Context = context;
            }

            public string GenerateToken(UserAccount userAccount)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);


                var claims = new List<Claim> 
                {
                    new Claim("email", userAccount.Email),
                    new Claim("id", userAccount.UserAccountId.ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddMinutes(15),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
                //var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                //    _configuration["Jwt:Audience"],
                //    claims,
                //    expires: DateTime.Now.AddMinutes(15),
                //    signingCredentials: credentials);


                //return new JwtSecurityTokenHandler().WriteToken(token);
            }

            public string GenerateRefreshToken()
            {
                var randomNumber = new byte[32];

                using (var generator = RandomNumberGenerator.Create())
                {
                    generator.GetBytes(randomNumber);
                    return Convert.ToBase64String(randomNumber);
                }
            }

            public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                return principal;
            }
        }
    }

