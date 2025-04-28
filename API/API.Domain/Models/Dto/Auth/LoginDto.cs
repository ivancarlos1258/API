using API.Domain.Models.Auth;
using AutoMapper;
using FluentValidation;

namespace API.Domain.Models.Dto
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Lembrar { get; set; }
        public string SenhaNova { get; set; }
        public string SenhaConferencia { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório!");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatório!");
        }
    }

    public class LoginDtoMapping : Profile
    {
        public LoginDtoMapping() => CreateMap<LoginDto, Login>();
    }
}
