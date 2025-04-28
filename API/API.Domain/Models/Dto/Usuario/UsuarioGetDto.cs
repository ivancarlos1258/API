using API.Domain.TableModels;
using AutoMapper;
using Gridify;

namespace API.Domain.Models.Dto
{
    public class UsuarioGetDto
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

        public Guid ClienteId { get; set; }

        public List<UsuarioGrupoGetDto> Grupos { get; } = new List<UsuarioGrupoGetDto>();
    }

    public class UsuarioGrupoGetDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public bool IsAtivo { get; set; }

        public List<UsuarioPermissaoGetDto> Permissoes { get; } = new List<UsuarioPermissaoGetDto>();
    }

    public class UsuarioPermissaoGetDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Chave { get; set; }

        public Guid SecaoPermissaoId { get; set; }

        public Guid? PermissaoMinima { get; set; }

        public UsuarioSecaoPermissaoGetDto SecaoPermissao { get; set; }
    }

    public class UsuarioSecaoPermissaoGetDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }
    }

    public class UsuarioGetMapping : Profile
    {
        public UsuarioGetMapping()
        {
            CreateMap<Usuario, UsuarioGetDto>().ReverseMap();
            CreateMap<Paging<Usuario>, Paging<UsuarioGetDto>>();
        }
    }
}
