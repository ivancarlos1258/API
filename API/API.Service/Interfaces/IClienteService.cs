using API.Domain.TableModels;
using Gridify;

namespace API.Service.Interfaces
{
    public interface IClienteService
    {
        Task<Cliente> ObterClientePorIdentificador(Guid identificador);
        Task<Paging<Cliente>> ListarClientes(GridifyQuery gridifyQuery);
        Task CadastrarCliente(Cliente cliente);
        Task AtualizarCliente(Cliente cliente);
        Task RemoverCliente(Guid identificador);
    }
}
