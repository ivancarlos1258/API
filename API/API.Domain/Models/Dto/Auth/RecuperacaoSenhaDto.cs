using API.Domain.Models.Auth;
using AutoMapper;

namespace API.Domain.Models.Dto
{
    public class RecuperacaoSenhaDto
    {
        public string Cpf { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
        public string SenhaAntiga { get; set; }
        public string SenhaNova { get; set; }
    }

    public class RecuperacaoSenhaDtoMapping : Profile
    {
        public RecuperacaoSenhaDtoMapping() => CreateMap<RecuperacaoSenhaDto, RecuperarSenha>();
    }
}
