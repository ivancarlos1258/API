namespace API.Domain.Models
{
    public class UsuarioLogado
    {
        public Guid? IdentificadorUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string[] Permissoes { get; set; }
        public bool IsPermissao(string permissao) => Permissoes.Any(x => x == permissao);
    }
}
