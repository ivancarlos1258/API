using System.Security.Claims;
using API.Domain.Models.Auth;
using API.Domain.Models.Dto;
using API.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Presentation.Controllers
{
    [ApiController]
    [Route("v1/Auth")]
    public class AuthController : Controller
    {
        private readonly IAutenticarService _autenticarService;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(
            IAutenticarService autenticarService,
            IUsuarioService usuarioService,
            ITokenService tokenService,
            IMapper mapper)
        {
            _autenticarService = autenticarService;
            _usuarioService = usuarioService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// Rota para gerar e obter o token
        /// </summary>
        /// <param name="key">Identificador do usuario</param>
        /// <returns>Token de autorização</returns>
        [ProducesResponseType(typeof(TokenGetDto), 200)]
        [HttpPost("GerarToken")]
        public async Task<IActionResult> GetToken([FromBody] Guid identificador)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdentificador(identificador);
            var token = await _tokenService.GenerateToken(usuario);
            var tokenDto = _mapper.Map<TokenGetDto>(token);
            return Ok(tokenDto);
        }

        /// <summary>
        /// Rota para autenticar usuário
        /// </summary>
        /// <param name="loginDto">Dados de login e senha</param>
        /// <returns>Lista de permissões</returns>
        [ProducesResponseType(typeof(string), 200)]
        [HttpPost("Autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] LoginDto loginDto)
        {
            try
            {
                var login = _mapper.Map<Login>(loginDto);
                var claims = await _autenticarService.Authenticate(login);
                return Ok(JsonConvert.SerializeObject(claims));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Rota para iniciar o processo de recuperação de senha e enviar o email para o usuário
        /// </summary>
        /// <param name="recuperacaoSenhaDto">Dados do usuário e senha</param>
        [HttpPost("SolicitarRecuperacaoSenha")]
        public async Task<IActionResult> SolicitarRecuperacaoSenha([FromBody] RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
            try
            {
                var recuperacaoSenha = _mapper.Map<RecuperarSenha>(recuperacaoSenhaDto);
                await _autenticarService.SolicitarRecuperacaoSenha(recuperacaoSenha);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Rota para redefinição de senha
        /// </summary>
        /// <param name="loginDto">Dados de login e senha</param>
        [HttpPost("RedefinirSenha")]
        public async Task<IActionResult> RedefinirSenha([FromBody] LoginDto loginDto)
        {
            try
            {
                var login = _mapper.Map<Login>(loginDto);
                await _autenticarService.RedefinirSenha(login);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Rota para iniciar o processo de recuperação de senha utilizando token e enviar o email para o usuário
        /// </summary>
        /// <param name="recuperacaoSenhaDto">Dados do usuário e senha</param>
        [HttpPost("RedefinirSenhaComToken")]
        public async Task<IActionResult> RedefinirSenhaComToken([FromBody] RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
            try
            {
                var recuperacaoSenha = _mapper.Map<RecuperarSenha>(recuperacaoSenhaDto);
                await _autenticarService.RedefinirSenhaComToken(recuperacaoSenha);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
