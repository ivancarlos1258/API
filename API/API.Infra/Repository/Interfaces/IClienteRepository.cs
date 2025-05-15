using API.Domain.TableModels;
using Gridify;

namespace API.Infra.Repository.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> ObterClientePorIdentificador(Guid identificador);
        Task<Cliente> ObterClientePorDocumento(string documento);
        Task<Paging<Cliente>> ListarClientes(GridifyQuery gridifyQuery);
        Task CadastrarCliente(Cliente cliente);
        Task AtualizarCliente(Cliente cliente);
        Task RemoveCliente(Guid codigocliente);
    }
}
