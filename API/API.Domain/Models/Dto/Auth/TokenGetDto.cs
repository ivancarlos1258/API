using API.Domain.Models.Auth;
using AutoMapper;

namespace API.Domain.Models.Dto
{
    public class TokenGetDto
    {
        public string IdentificadorToken { get; set; }
        public DateTime? ExpiraEm { get; set; }
    }

    public class TokenGetMapping : Profile
    {
        public TokenGetMapping() => CreateMap<Token, TokenGetDto>()
            .ForMember(x => x.IdentificadorToken, opt => opt.MapFrom(src => src.IdentificadorToken))
            .ForMember(x => x.ExpiraEm, opt => opt.MapFrom(src => src.ExpiraEm));
    }
}
