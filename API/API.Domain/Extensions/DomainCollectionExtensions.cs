using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Domain
{
    public class InjecaoDependenciaDomain
    {
        public static void RegistrarServicos(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
