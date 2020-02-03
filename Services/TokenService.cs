using System;
using System.Text;
using System.Security.Claims;
using my_time_api.Model;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace my_time_api.Services
{
    public class TokenService {

        private readonly IConfiguration _config;
        public TokenService(IConfiguration config){
            _config = config;
        }        

        public string GenerateToken(User user){

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["hashJwt"]);
            var tokenDescriptor  = new SecurityTokenDescriptor
             {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Role, user.Role == null ? "User" : user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}