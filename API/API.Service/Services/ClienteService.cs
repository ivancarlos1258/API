using System.ComponentModel.DataAnnotations;
using API.Domain.TableModels;
using API.Infra.Repository.Interfaces;
using API.Service.Interfaces;
using Gridify;

namespace API.Service.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(
            IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente> ObterClientePorIdentificador(Guid identificador)
        {
            return await _clienteRepository.ObterClientePorIdentificador(identificador);
        }

        public async Task<Paging<Cliente>> ListarClientes(GridifyQuery gridifyQuery)
        {
            return await _clienteRepository.ListarClientes(gridifyQuery);
        }

        public async Task CadastrarCliente(Cliente cliente)
        {
            var clienteExistente = await _clienteRepository.ObterClientePorDocumento(cliente.Documento);

            if (clienteExistente is not null) 
            {
                throw new ValidationException("Já existe um cliente cadastrado com esse documento!");
            }

            await _clienteRepository.CadastrarCliente(cliente);
        }

        public async Task AtualizarCliente(Cliente cliente)
        {
            var clienteExistente = await _clienteRepository.ObterClientePorIdentificador(cliente.CodigoCliente);

            if (clienteExistente is null)
            {
                throw new ValidationException("Cliente informado não existe!");
            }

            if (clienteExistente.Documento != cliente.Documento 
                && await _clienteRepository.ObterClientePorDocumento(cliente.Documento) is not null) 
            {
                throw new ValidationException("Já existe um cliente com esse documento");
            }

            clienteExistente.NomeFantasia = cliente.NomeFantasia;
            //Fazer para todos os campos


            await _clienteRepository.AtualizarCliente(clienteExistente);
        }

        public async Task RemoverCliente(Guid codicocliente)
        {
           
            await _clienteRepository.RemoveCliente(codicocliente);
        }
    }
}
