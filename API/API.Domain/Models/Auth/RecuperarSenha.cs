namespace API.Domain.Models.Auth
{
    public class RecuperarSenha
    {
        public string Cpf { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
        public string SenhaAntiga { get; set; }
        public string SenhaNova { get; set; }
    }
}
