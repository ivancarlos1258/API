using Microsoft.Extensions.Configuration;

namespace API.Utility
{
    public class Configuracao
    {
        private readonly IConfiguration appSettings;

        public IConfiguration AppSettings => this.appSettings;

        public Configuracao()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(env))
                throw new InvalidOperationException("Para ter acesso a este recurso, e preciso ter a váriavel ASPNETCORE_ENVIRONMENT cadastrado em sua máquina.");

            try
            {
                this.appSettings = (IConfiguration)new ConfigurationBuilder().AddJsonFile(
                    "appsettings.json", true, false).AddJsonFile($"appsettings.{env}.json", true, false).AddEnvironmentVariables().Build();
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("Verifique o formato do arquivo Json: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao carregar o arquivo de configuração: " + ex.Message);
            }
        }
    }
}
