using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Domain.Models.Auth;
using API.Domain.TableModels;
using API.Service.Interfaces;
using API.Utility;
using Microsoft.IdentityModel.Tokens;

namespace API.Service.Services
{
    public class TokenService : ITokenService 
    {
        public async Task<Token> GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(new Configuracao().AppSettings["SecretKey"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, usuario.Nome),
                    new(ClaimTypes.Role, usuario.IsAdministrador ? "Admin" : "Padrão"),
                    new("UsuarioId", usuario.Id.ToString())
                   
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ) 
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor); 

            return new Token() { 
                IdentificadorToken = tokenHandler.WriteToken(token), 
                ExpiraEm = tokenDescriptor.Expires 
            };
        } 
    }
}
