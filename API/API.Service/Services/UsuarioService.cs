using System.ComponentModel.DataAnnotations;
using API.Domain.Models;
using API.Domain.TableModels;
using API.Infra.Repository.Interfaces;
using API.Service.Interfaces;
using API.Service.Utility;
using API.Utility;
using Gridify;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Service.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UsuarioLogado _usuarioLogado;
        private readonly ApplicationSettings _config;

        public UsuarioService(
            IHttpContextAccessor accessor,
            IUsuarioRepository usuarioRepository, 
            IOptions<ApplicationSettings> options)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioLogado = DadosUsuarioLogado.ObterDadosUsuarioLogado(accessor.HttpContext.User.Claims);
            _config = options.Value;
        }

        public async Task<List<Usuario>> ListarUsuarios(bool trazerInativos = false)
        {
            return await _usuarioRepository.ListarUsuarios(trazerInativos);
        }

        public async Task<Paging<Usuario>> ListarUsuarios(GridifyQuery gridifyQuery)
        {
            return await _usuarioRepository.ListarUsuarios(gridifyQuery);
        }

        public async Task<Usuario> ObterUsuarioPorEmail(string email)
        {
            return await _usuarioRepository.ObterUsuarioPorEmail(email);
        }

        public async Task<Usuario> ObterUsuarioPorEmailCpf(string email, string cpf)
        {
            return await _usuarioRepository.ObterUsuarioPorEmailCpf(email, cpf);
        }

        public async Task<bool> VerificarExistenciaUsuarioPorEmailSenha(string email, string senha)
        {
            var usuario = await _usuarioRepository.ObterUsuarioPorEmailSenha(email, senha);
            return usuario is not null;
        }

        public async Task CadastrarUsuario(Usuario usuario)
        {
            PadronizarEntrada(usuario);

            if(await _usuarioRepository.ObterUsuarioPorEmail(usuario.Email) is not null)
                throw new ValidationException("E-mail informado já está em uso.");

            if (await _usuarioRepository.ObterUsuarioPorCpf(usuario.Cpf) is not null)
                throw new ValidationException("CPF informado já está em uso.");

            var novaSenha = Criptografia.GerarSenha(10, 3);

            usuario.Senha = Criptografia.StringParaSha512(novaSenha);

            await _usuarioRepository.CadastrarUsuario(usuario);

            Email.EnviarEmail(usuario.Email, "G-Bov - Sua nova senha de acesso",
                HtmlSnippets.CriarEmailTemplateHtml("recuperacaoAcesso.html",
                new Dictionary<string, string>
            {
                { "{NOME-USUARIO}", usuario.Nome },
                { "{SENHA-NOVA}", novaSenha },
                { "{URL-PAINEL}", _config.UrlApp }
            }), true);
        }

        public async Task AtualizarUsuario(Usuario usuario)
        {
            PadronizarEntrada(usuario);

            var usuarioExistente = await _usuarioRepository.ObterUsuarioPorIdentificador(usuario.Id);

            if (usuarioExistente is null)
                throw new ValidationException("Usuário informado não existe.");

            if (usuarioExistente.IsAdministrador)
                throw new ValidationException("O super usuário não pode ser alterado.");

            if (usuarioExistente.Email != usuario.Email && _usuarioRepository.ObterUsuarioPorEmail(usuario.Email) is not null)
                throw new ValidationException("E-mail informado já está em uso.");

            var isPermissaoEditarCpfCnpj = _usuarioLogado.IsPermissao("298b5b16-0273-44cf-9555-0214e43658a0");

            if (usuarioExistente.Cpf != usuario.Cpf
                && !isPermissaoEditarCpfCnpj)
                throw new ValidationException("Permissão insuficiente para editar o CPF do usuário.");

            if (usuarioExistente.Cpf != usuario.Cpf && _usuarioRepository.ObterUsuarioPorCpf(usuario.Cpf) is not null)
                throw new ValidationException("CPF informado já está em uso.");

            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Cpf = usuario.Cpf;
            usuarioExistente.Nascimento = usuario.Nascimento;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Telefone = usuario.Telefone;

            await _usuarioRepository.AtualizarUsuario(usuarioExistente);
        }

        public async Task<Usuario> ObterUsuarioPorIdentificador(Guid identificadorUsuario)
        {
            return await _usuarioRepository.ObterUsuarioPorIdentificador(identificadorUsuario);
        }

        public async Task AtivarDesativarUsuario(Guid identificadorUsuario)
        {
            var usuarioExistente = await _usuarioRepository.ObterUsuarioPorIdentificador(identificadorUsuario);

            if (usuarioExistente is null)
                throw new ValidationException("Usuário informado não existe.");

            if (usuarioExistente.IsAdministrador)
                throw new ValidationException("O super usuário não pode ser alterado.");

            usuarioExistente.IsAtivo = !usuarioExistente.IsAtivo;

            await _usuarioRepository.AtualizarUsuario(usuarioExistente);
        }

        public async Task AtualizarSenhaUsuario(Guid identificadorUsuario, string novaSenha)
        {
            var usuarioExistente = await _usuarioRepository.ObterUsuarioPorIdentificador(identificadorUsuario);

            if (usuarioExistente is null)
                throw new ValidationException("Usuário informado não existe.");

            if (!usuarioExistente.IsAtivo)
                throw new ValidationException("Não é possível alterar a senha de um usuário inativo.");

            usuarioExistente.Senha = Criptografia.StringParaSha512(novaSenha);
            usuarioExistente.IsRedefinirSenha = false;
            usuarioExistente.TokenRecuperacaoSenha = null;
            usuarioExistente.ValidadeRecuperacaoSenha = null;

            await _usuarioRepository.AtualizarUsuario(usuarioExistente);
        }

        public async Task SolicitarRedefinicaoSenha(Guid identificadorUsuario)
        {
            var usuarioExistente = await _usuarioRepository.ObterUsuarioPorIdentificador(identificadorUsuario);

            if (usuarioExistente is null)
                throw new ValidationException("Usuário informado não existe.");

            if (usuarioExistente.IsAdministrador)
                throw new ValidationException("O super usuário não pode ser alterado.");

            if (!usuarioExistente.IsAtivo)
                throw new ValidationException("Não é possível solicitar a alteração de senha de um usuário inativo.");

            var novaSenha = Criptografia.GerarSenha(10, 3);
            usuarioExistente.Senha = Criptografia.StringParaSha512(novaSenha);
            usuarioExistente.IsRedefinirSenha = true;

            await _usuarioRepository.AtualizarUsuario(usuarioExistente);

            Email.EnviarEmail(usuarioExistente.Email, "G-Bov - Sua nova senha de acesso",
                HtmlSnippets.CriarEmailTemplateHtml("recuperacaoAcesso.html",
                new Dictionary<string, string>
            {
                { "{NOME-USUARIO}", usuarioExistente.Nome },
                { "{SENHA-NOVA}", novaSenha },
                { "{URL-PAINEL}", _config.UrlApp }
            }), true);
        }

        private static void PadronizarEntrada(Usuario usuario) 
        {
            usuario.Nome = usuario.Nome.Trim().ToUpper();
            usuario.Cpf = Conversoes.SomenteNumeros(usuario.Cpf.Trim());
            usuario.Email = usuario.Email.Trim().ToLower();
        }
    }
}
