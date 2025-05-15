using System.Reflection;
using Api.Infra.Repository;
using API.Infra;
using API.Infra.Repository;
using API.Infra.Repository.Interfaces;
using API.Infra.Utility;
using API.Infra.Utility.Interfaces;
using API.Service.Interfaces;
using API.Service.Services;
using API.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Service
{
    public class InjecaoDependenciaService
    {
        public static void RegistrarServicos(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<Context>(
                options => options.UseNpgsql(new Configuracao().AppSettings["ConexaoPostgresql"]));

         

            services.AddScoped<IAutenticarService, AutenticarService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentClienteService, CurrentClienteService>();
           
           
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();


        }
    }
}
