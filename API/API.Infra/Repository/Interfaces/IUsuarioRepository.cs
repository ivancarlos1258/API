using API.Domain.TableModels;
using Gridify;

namespace API.Infra.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObterUsuarioPorEmail(string email);
        Task<Usuario> ObterUsuarioPorEmailSenha(string email, string senha);
        Task<List<Usuario>> ListarUsuarios(bool trazerInativos = false);
        Task<Paging<Usuario>> ListarUsuarios(GridifyQuery gridifyQuery);
        Task<Usuario> ObterUsuarioPorIdentificador(Guid identificador);
        Task AtualizarUsuario(Usuario model);
        Task CadastrarUsuario(Usuario model);
        Task<Usuario> ObterUsuarioPorCpf(string cpf);
        Task<Usuario> ObterUsuarioPorEmailCpf(string email, string cpf);
        
    }
}
