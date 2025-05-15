using System.Security.Claims;
using API.Domain.Models;

namespace API.Service.Utility
{
    public static class DadosUsuarioLogado
    {
        public static UsuarioLogado ObterDadosUsuarioLogado(IEnumerable<Claim> claims)
        {
            var dadosUsuario = ObterDadosUsuario(claims);
            return dadosUsuario;
        }

        private static UsuarioLogado ObterDadosUsuario(IEnumerable<Claim> claims)
        {
            try
            {
                return new UsuarioLogado()
                {
                    IdentificadorUsuario = claims.Any(x => x.Type == "IdentificadorUsuario") ? Guid.Parse(claims.First(x => x.Type == "IdentificadorUsuario").Value) : null,
                    Nome = claims.Any(x => x.Type == "Nome") ? claims.First(x => x.Type == "Nome").Value : null,
                    Email = claims.Any(x => x.Type == "Email") ? claims.First(x => x.Type == "Email").Value : null,
                    Permissoes = claims.Any(x => x.Type == "Permissoes") ? claims.First(x => x.Type == "Permissoes").Value.Split(';') : null
                };
            }
            catch (Exception)
            {
                throw new InvalidCastException("Não foi possível recuperar os valores defaults do usuário logado.");
            }
        }
    }
}
