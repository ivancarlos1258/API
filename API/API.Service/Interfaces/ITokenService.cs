using API.Domain.Models.Auth;
using API.Domain.TableModels;

namespace API.Service.Interfaces
{
    public interface ITokenService
    {
        Task<Token> GenerateToken(Usuario usuario);
    }
}
