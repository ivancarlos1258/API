using API.Domain.Models.Dto;
using API.Domain.TableModels;
using API.Service.Interfaces;
using AutoMapper;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("v1/Usuario")]
    public class UserController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UserController(
            IUsuarioService usuarioService,
            IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtem os dados do usuário
        /// </summary>
        /// <param name="identificador">Identificador do usuário</param>
        /// <returns>Dados do usuário</returns>
        [HttpGet("{identificador}")]
        [ProducesResponseType(typeof(UsuarioGetDto), 200)]
        public async Task<IActionResult> ObterUsuarioPorIdentificador([FromRoute] Guid identificador)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdentificador(identificador);
            var usuarioGetDto = _mapper.Map<UsuarioGetDto>(usuario);
            return Ok(usuarioGetDto);
        }

        /// <summary>
        /// Obtem uma lista paginada de usuários
        /// </summary>
        /// <param name="gridifyQuery">Filtros de query</param>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paging<UsuarioGetDto>), 200)]
        public async Task<IActionResult> ListarUsuariosPaginado([FromQuery] GridifyQuery gridifyQuery)
        {
            var usuarios = await _usuarioService.ListarUsuarios(gridifyQuery);
            var usuariosGetDto = _mapper.Map<Paging<UsuarioGetDto>>(usuarios);
            return Ok(usuariosGetDto);
        }

        /// <summary>
        /// Salva os dados de um novo usuário
        /// </summary>
        /// <param name="usuarioPostDto">Dados do usuário</param>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario([FromBody] UsuarioPostDto usuarioPostDto)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(usuarioPostDto);
                await _usuarioService.CadastrarUsuario(usuario);
                return Ok(usuario);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza os dados do usuário
        /// </summary>
        /// <param name="usuarioPutDto">Dados do usuário</param>
        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario([FromBody] UsuarioPutDto usuarioPutDto)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(usuarioPutDto);
                await _usuarioService.AtualizarUsuario(usuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Altera a chave isAtivo para ativar ou inativar um usuário
        /// </summary>
        /// <param name="identificador">Identificador do usuário</param>
        [HttpPut("AtivarDesativar/{identificador}")]
        public async Task<IActionResult> AtivarDesativarUsuario([FromRoute] Guid identificador)
        {
            try
            {
                await _usuarioService.AtivarDesativarUsuario(identificador);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Envia o email para redefinição de senha
        /// </summary>
        /// <param name="identificador">Identificador do usuário</param>
        [HttpPut("SolicitarAlteracaoSenha/{identificador}")]
        public async Task<IActionResult> SolicitarRedefinicaoSenhaUsuario([FromRoute] Guid identificador)
        {
            try
            {
                await _usuarioService.SolicitarRedefinicaoSenha(identificador);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Altera a senha do usuário
        /// </summary>
        /// <param name="identificador">Identificador do usuário</param>
        /// <param name="senha">Nova senha</param>
        [HttpPut("AtualizarSenha")]
        public async Task<IActionResult> AtualizarSenhaUsuario([FromQuery] Guid identificador, string senha)
        {
            try
            {
                await _usuarioService.AtualizarSenhaUsuario(identificador, senha);
                return Ok();
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }
    }
}
