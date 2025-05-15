namespace API.Domain.Models.Auth
{
    public class Token
    {
        public string IdentificadorToken { get; set; }
        public DateTime? ExpiraEm {  get; set; }
    }
}
