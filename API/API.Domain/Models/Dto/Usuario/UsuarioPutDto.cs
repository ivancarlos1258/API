using API.Domain.TableModels;
using API.Utility;
using AutoMapper;
using FluentValidation;

namespace API.Domain.Models.Dto
{
    public class UsuarioPutDto
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

        public List<UsuarioGrupoPutDto> Grupos { get; set; } = new List<UsuarioGrupoPutDto>();
    }

    public class UsuarioGrupoPutDto
    {
        public Guid Id { get; set; }
    }

    public class UsuarioPutValidator : AbstractValidator<UsuarioPutDto>
    {
        public UsuarioPutValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MinimumLength(5).WithMessage("Nome inválido.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .Must(x => Validacao.ValidarEmail(x)).WithMessage("E-mail inválido");

            RuleFor(x => x.Cpf)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Must(x => !Validacao.ValidarCpf(x)).WithMessage("CPF inválido.");

            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .Length(14, 15).WithMessage("Telefone inválido.");
        }
    }

    public class UsuarioPutMapping : Profile
    {
        public UsuarioPutMapping() => CreateMap<UsuarioPutDto, Usuario>();
    }
}
