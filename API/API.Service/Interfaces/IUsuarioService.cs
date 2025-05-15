using API.Domain.TableModels;
using Gridify;

namespace API.Service.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ListarUsuarios(bool trazerInativos = false);

        Task<Paging<Usuario>> ListarUsuarios(GridifyQuery gridifyQuery);

        Task<Usuario> ObterUsuarioPorEmail(string email);

        Task<Usuario> ObterUsuarioPorIdentificador(Guid identificadorUsuario);

        Task<Usuario> ObterUsuarioPorEmailCpf(string email, string cpf);

        Task<bool> VerificarExistenciaUsuarioPorEmailSenha(string email, string senha);

        Task CadastrarUsuario(Usuario usuario);

        Task AtualizarUsuario(Usuario usuario);

        Task AtivarDesativarUsuario(Guid identificadorUsuario);

        Task AtualizarSenhaUsuario(Guid identificadorUsuario, string novaSenha);

        Task SolicitarRedefinicaoSenha(Guid identificadorUsuario);
    }
}
