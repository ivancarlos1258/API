using API.Domain.TableModels;
using API.Infra.Repository.Interfaces;
using Gridify.EntityFramework;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace API.Infra.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly Context _context;

        public UsuarioRepository(Context context) 
        {
            _context = context;
        }

        public async Task<List<Usuario>> ListarUsuarios(bool trazerInativos = false)
        {
            var retorno = _context.Usuario.Where(x => x.IsAtivo == !trazerInativos).OrderBy(x => x.Email);
            await retorno.ForEachAsync(x => x.Senha = null);
            return await retorno.ToListAsync();
        }

        public async Task<Paging<Usuario>> ListarUsuarios(GridifyQuery gridifyQuery)
        {
            var retorno = await _context.Usuario
                .GridifyAsync(gridifyQuery);
            foreach (Usuario usuario in retorno.Data)
            {
                usuario.Senha = null;
            }
            return retorno;
        }

        public async Task<Usuario> ObterUsuarioPorEmail(string email)
        {
            return await _context.Usuario.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Usuario> ObterUsuarioPorEmailSenha(string email, string senha)
        {
            return await _context.Usuario.FirstOrDefaultAsync(x => x.Email == email && x.Senha == senha);
        }

        public async Task<Usuario> ObterUsuarioPorIdentificador(Guid identificador)
        {
            return await _context.Usuario.FirstOrDefaultAsync(x => x.Id == identificador);
        }

        public async Task AtualizarUsuario(Usuario model)
        {
            _context.Usuario.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task CadastrarUsuario(Usuario model)
        {
            _context.Usuario.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> ObterUsuarioPorCpf(string cpf)
        {
            return await _context.Usuario.FirstOrDefaultAsync(x => x.Cpf == cpf && x.IsAtivo);
        }

        public async Task<Usuario> ObterUsuarioPorEmailCpf(string email, string cpf)
        {
            return await _context.Usuario.FirstOrDefaultAsync(x => x.Email == email && x.Cpf == cpf);
        }

    }
}
