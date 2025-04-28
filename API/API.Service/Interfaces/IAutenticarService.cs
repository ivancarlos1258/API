using API.Domain.Models.Auth;
using API.Domain.TableModels;
using System.Security.Claims;

namespace API.Service.Interfaces
{
    public interface IAutenticarService
    {
        Task<List<Claim>> Authenticate(Usuario dadosUsuario, bool isLembrar);
        Task<List<Claim>> Authenticate(Login model);
        Task RedefinirSenha(Login login);
        Task RedefinirSenhaComToken(RecuperarSenha recuperarSenha);
        Task SolicitarRecuperacaoSenha(RecuperarSenha recuperarSenha);
    }
}
