using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using API.Domain.Models.Auth;
using API.Domain.Models;
using API.Domain.TableModels;
using API.Service.Interfaces;
using API.Utility;
using Microsoft.Extensions.Options;

namespace API.Service.Services
{
    public class AutenticarService : IAutenticarService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ApplicationSettings _config;

        public AutenticarService(
            IUsuarioService usuarioService,
            IOptions<ApplicationSettings> options)
        {
            _usuarioService = usuarioService;
            _config = options.Value;
        }

        public async Task<List<Claim>> Authenticate(Login login)
        {
            var usuarioExiste = await _usuarioService.VerificarExistenciaUsuarioPorEmailSenha(login.Email, login.Senha);

            if (!usuarioExiste)
                throw new ValidationException("Usuário ou senha inválidos.");

            var dadosUsuario = await _usuarioService.ObterUsuarioPorEmail(login.Email);

            return await Autenticar(dadosUsuario, login.Lembrar);
        }

        public async Task<List<Claim>> Authenticate(Usuario dadosUsuario, bool isLembrar)
        {
            return await Autenticar(dadosUsuario, isLembrar);
        }

        public async Task RedefinirSenha(Login login)
        {
            var usuarioExiste = await _usuarioService.VerificarExistenciaUsuarioPorEmailSenha(login.Email, login.Senha);
            if (!usuarioExiste)
                throw new ValidationException("Usuário ou senha inválidos.");

            await RedefinirSenhaGenerico(login);
        }

        public async Task SolicitarRecuperacaoSenha(RecuperarSenha recuperarSenha)
        {
            recuperarSenha.Email = recuperarSenha.Email.ToLower().Trim();
            recuperarSenha.Cpf = Conversoes.SomenteNumeros(recuperarSenha.Cpf);

            var usuarioExistente = await _usuarioService.ObterUsuarioPorEmailCpf(recuperarSenha.Email, recuperarSenha.Cpf);

            if (usuarioExistente is null)
                throw new ValidationException("Não foi possível enviar a recuperação de senha. Combinação de e-mail e cpf não corresponde a nenhum usuário do sistema.");

            if (!usuarioExistente.IsAtivo)
                throw new ValidationException("Não e possível solicitar recuperação de senha para um usuário inativo.");

            usuarioExistente.ValidadeRecuperacaoSenha = Conversoes.DateTimeBr(DateTime.Now).AddMinutes(30);
            usuarioExistente.TokenRecuperacaoSenha = Guid.NewGuid();
            usuarioExistente.IsRedefinirSenha = true;

            await _usuarioService.AtualizarUsuario(usuarioExistente);

            Email.EnviarEmail(usuarioExistente.Email, "G-Bov - Recuperação de senha",
                HtmlSnippets.CriarEmailTemplateHtml("recuperacaoSenha.html",
                new Dictionary<string, string>
            {
                { "{NOME-USUARIO}", usuarioExistente.Nome },
                { "{TOKEN}", usuarioExistente.TokenRecuperacaoSenha.ToString() },
                { "{URL-PAINEL}", _config.UrlApp }
            }), true);
        }

        public async Task RedefinirSenhaComToken(RecuperarSenha recuperarSenha)
        {
            if (recuperarSenha == null)
                throw new ValidationException("Token inválido.");

            recuperarSenha.Email = recuperarSenha.Email.ToLower().Trim();
            recuperarSenha.Cpf = Conversoes.SomenteNumeros(recuperarSenha.Cpf); 

            var usuarioExistente = await _usuarioService.ObterUsuarioPorEmailCpf(recuperarSenha.Email, recuperarSenha.Cpf);

            if (usuarioExistente is null
                || recuperarSenha.Token != usuarioExistente.TokenRecuperacaoSenha
                || usuarioExistente.TokenRecuperacaoSenha is null
                || usuarioExistente.ValidadeRecuperacaoSenha < Conversoes.DateTimeBr(DateTime.Now))
                throw new ValidationException("Token de recuperação inválido ou expirado.");

            await RedefinirSenhaGenerico(new Login
            {
                Email = recuperarSenha.Email,
                SenhaConferencia = recuperarSenha.SenhaAntiga,
                SenhaNova = recuperarSenha.SenhaNova
            });
        }

        private async Task<List<Claim>> Autenticar(Usuario dadosUsuario, bool isLembrar)
        {
            if (!dadosUsuario.IsAtivo)
                throw new ValidationException("Usuário desativado.");

            if (dadosUsuario.IsRedefinirSenha)
                throw new ValidationException("Senha expirada, favor realizar a troca.");

            var claims = new List<Claim>
            {
                new Claim("IdentificadorUsuario", dadosUsuario.Id.ToString()),
                new Claim("Nome", dadosUsuario.Nome),
                new Claim("Email", dadosUsuario.Email),
                new Claim("DataLogin", Conversoes.DateTimeBr(DateTime.Now).ToString()),
                new Claim("IsLembrar", isLembrar.ToString()),
                new Claim("IsAdministrador", dadosUsuario.IsAdministrador.ToString())
            };
          
            return claims;
        }

        private async Task RedefinirSenhaGenerico(Login login)
        {
            var dadosUsuario = await _usuarioService.ObterUsuarioPorEmail(login.Email);

            if (!dadosUsuario.IsAtivo)
                throw new ValidationException("Usuário desativado.");

            if (!dadosUsuario.IsRedefinirSenha)
                throw new ValidationException("Não foi solicitado a redifinição de senha para o seu usuário.");

            if (login.SenhaNova != login.SenhaConferencia)
                throw new ValidationException("As senhas digitadas não conferem.");

            if (login.SenhaNova.Length < 10)
                throw new ValidationException("As senha precisa ter pelo menos 10 caracteres.");

            await _usuarioService.AtualizarSenhaUsuario(dadosUsuario.Id, login.SenhaNova);
        }
    }
}
