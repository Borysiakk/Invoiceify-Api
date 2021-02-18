using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Invoiceify.Domain.Entities;
using Invoiceify.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Invoiceify.Infrastructure.Services
{
    public class JwtTokenService :ITokenService
    {
        private readonly SymmetricSecurityKey _key;
     
        public JwtTokenService(IConfiguration configuration )
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
        }
        
        public string Generate(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var creds = new SigningCredentials(_key,  SecurityAlgorithms.HmacSha256Signature);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = creds,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}