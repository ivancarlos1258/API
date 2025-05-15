using API.Domain.Models.Dto;
using API.Domain.TableModels;
using API.Service.Interfaces;
using AutoMapper;
using Gridify;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [ApiController]
    [Route("v1/Cliente")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(
            IClienteService clienteService,
            IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtem os dados do cliente
        /// </summary>
        /// <param name="identificador">Identificador do cliente</param>
        /// <returns>Dados do cliente</returns>
        [HttpGet("{identificador}")]
        [ProducesResponseType(typeof(ClienteGetDto), 200)]
        public async Task<IActionResult> ObterClientePorIdentificador([FromRoute] Guid identificador)
        {
            var cliente = await _clienteService.ObterClientePorIdentificador(identificador);
            var clienteGetDto = _mapper.Map<ClienteGetDto>(cliente);
            return Ok(clienteGetDto);
        }

        /// <summary>
        /// Obtem uma lista paginada de clientes
        /// </summary>
        /// <param name="gridifyQuery">Filtros de query</param>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Paging<ClienteGetDto>), 200)]
        public async Task<IActionResult> ListarClientesPaginado([FromQuery] GridifyQuery gridifyQuery)
        {
            var clientes = await _clienteService.ListarClientes(gridifyQuery);
            var clientesGetDto = _mapper.Map<Paging<ClienteGetDto>>(clientes);
            return Ok(clientesGetDto);
        }

        /// <summary>
        /// Salva os dados de um novo cliente
        /// </summary>
        /// <param name="clientePostDto">Dados do cliente</param>
        [HttpPost]
        public async Task<IActionResult> CadastrarCliente([FromBody] ClientePostDto clientePostDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clientePostDto);
                await _clienteService.CadastrarCliente(cliente);
                return Ok(cliente);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza os dados do cliente
        /// </summary>
        /// <param name="clientePutDto">Dados do cliente</param>
        [HttpPut]
        public async Task<IActionResult> AtualizarCliente([FromBody] ClientePutDto clientePutDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clientePutDto);
                await _clienteService.AtualizarCliente(cliente);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove o cliente
        /// </summary>
        /// <param name="codigocliente">Identificador do cliente</param>
        [HttpPut("RemoverCliente/{codigocliente}")]
        public async Task<IActionResult> RemoverCliente([FromRoute] Guid codigocliente)
        {
            try
            {
                await _clienteService.RemoverCliente(codigocliente);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
