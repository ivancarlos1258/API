using System.Reflection;
using API.Domain.TableModels;
using API.Infra.Utility.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Infra
{
    public class Context : DbContext
    {
        private readonly ICurrentClienteService _currentClienteService;

        public Context(
            DbContextOptions options,
            ICurrentClienteService currentClienteService) : base(options) 
        {
            _currentClienteService = currentClienteService;
        }

        protected Guid _clienteId => _currentClienteService.clienteId ?? Guid.Empty;
       
        public DbSet<Usuario> Usuario => Set<Usuario>();
        public DbSet<Cliente> Cliente => Set<Cliente>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
         
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           
            return (await base.SaveChangesAsync(true, cancellationToken));
        }
    }
}
