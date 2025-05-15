
using API.Domain.TableModels;
using API.Utility;
using AutoMapper;
using FluentValidation;

namespace API.Domain.Models.Dto
{
    public class ClientePostDto
    {
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string TipoPessoa { get; set; }
        public string Documento { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Uf { get; set; }
        public DateTime DataInsercao { get; set; }
        public string UsuarioInsercao { get; set; }
    }

    public class ClientePostValidator : AbstractValidator<ClientePostDto>
    {
        public ClientePostValidator()
        {
            RuleFor(x => x.Documento)
                .Custom((documento, context) =>
                {
                    if (string.IsNullOrEmpty(documento))
                    {
                        context.AddFailure("O documento é obrigatório");
                    }
                    else if (documento.Length != 11 && documento.Length != 14)
                    {
                        context.AddFailure("Documento inválido");
                    }
                    else if (documento.Length == 11 && !Validacao.ValidarCpf(documento))
                    {
                        context.AddFailure("CPF inválido");
                    }
                    else if (documento.Length == 14 && !Validacao.ValidarCnpj(documento))
                    {
                        context.AddFailure("CNPJ inválido");
                    }
                });

        }
    }

    public class ClientePostMapping : Profile
    {
        public ClientePostMapping() => CreateMap<ClientePostDto, Cliente>();
    }
}
