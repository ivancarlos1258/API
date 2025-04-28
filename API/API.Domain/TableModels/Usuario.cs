namespace API.Domain.TableModels
{
    public partial class Usuario 
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Cpf { get; set; }

        public DateTime Nascimento { get; set; }

        public string Telefone { get; set; }

        public bool IsAdministrador { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsRedefinirSenha { get; set; }

        public Guid? TokenRecuperacaoSenha { get; set; }

        public DateTime? ValidadeRecuperacaoSenha { get; set; }

    }
}
