using API.Domain.TableModels;
using API.Infra;
using API.Infra.Repository.Interfaces;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Api.Infra.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly Context _context;
        public ClienteRepository(Context context) 
        {
            _context = context;
        }

        public async Task<Cliente> ObterClientePorIdentificador(Guid codigocliente)
        {
            return await _context.Cliente.FirstOrDefaultAsync(x => x.CodigoCliente == codigocliente);
        }

        public async Task<Cliente> ObterClientePorDocumento(string documento)
        {
            return await _context.Cliente.FirstOrDefaultAsync(x => x.Documento == documento);
        }

        public async Task<Paging<Cliente>> ListarClientes(GridifyQuery gridifyQuery)
        {
            return await _context.Cliente.GridifyAsync(gridifyQuery);
        }

        public async Task CadastrarCliente(Cliente cliente)
        {
            await _context.Cliente.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarCliente(Cliente cliente)
        {
            _context.Cliente.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCliente(Guid codigocliente)
        {
            var item = await _context.Cliente.FirstOrDefaultAsync(x => x.CodigoCliente == codigocliente);
            if (item == null)
                throw new Exception("Cliente não encontrado");
            _context.Cliente.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
