using API.Domain.TableModels;
using AutoMapper;
using Gridify;

namespace API.Domain.Models.Dto
{
    public class ClienteGetDto
    {
        public Guid CodigoCliente { get; set; }
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

    public class ClienteGetMapping : Profile
    {
        public ClienteGetMapping()
        {
            CreateMap<Cliente, ClienteGetDto>().ReverseMap();
            CreateMap<Paging<Cliente>, Paging<ClienteGetDto>>();
        }
    }
}
