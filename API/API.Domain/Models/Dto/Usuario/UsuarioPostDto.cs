using API.Domain.TableModels;
using API.Utility;
using AutoMapper;
using FluentValidation;

namespace API.Domain.Models.Dto
{
    public class UsuarioPostDto
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Cpf { get; set; }

        public DateTime Nascimento { get; set; }

        public string Telefone { get; set; }

        public Guid ClienteId { get; set; }

        public bool isAtivo { get; set; }

        //public List<UsuarioGrupoPostDto> Grupos { get; set; } = new List<UsuarioGrupoPostDto>();
    }

    public class UsuarioGrupoPostDto
    {
        public Guid Id { get; set; }
    }

    public class UsuarioPostValidator : AbstractValidator<UsuarioPostDto>
    {
        public UsuarioPostValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MinimumLength(5).WithMessage("Nome inválido.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .Must(x => Validacao.ValidarEmail(x)).WithMessage("E-mail inválido");

            //RuleFor(x => x.Cpf)
            //    .NotEmpty().WithMessage("CPF é obrigatório.")
            //    .Must(x => Validacao.ValidarCpf(x)).WithMessage("CPF inválido.");

            //RuleFor(x => x.Telefone)
            //    .NotEmpty().WithMessage("Telefone é obrigatório.")
            //    .Length(10, 11).WithMessage("Telefone inválido.");
        }
    }

    public class UsuarioPostMapping : Profile
    {
        public UsuarioPostMapping() => CreateMap<UsuarioPostDto, Usuario>();
    }
}
