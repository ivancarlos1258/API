using System.Security.Claims;
using API.Infra.Utility.Interfaces;
using Microsoft.AspNetCore.Http;

namespace API.Infra.Utility
{
    public class CurrentClienteService : ICurrentClienteService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public string? userId => GetUserId();
        public Guid? clienteId => GetClienteId();

        public CurrentClienteService(
            IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetUserId()
        {
            var user = _contextAccessor.HttpContext?.User;

            if (user == null) 
            {
                return null;
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) 
            {
                userId = user.FindFirstValue("sub");
            }

            return userId;
        }

        public Guid? GetClienteId()
        {
            var clienteId = _contextAccessor.HttpContext?.User?.Claims?
                .FirstOrDefault(c => c.Type == "ClienteId");

            if (clienteId == null) 
            {
                return (Guid?)null;
            }

            return !string.IsNullOrEmpty(clienteId.Value) ? Guid.Parse(clienteId.Value) : (Guid?)null;
        }
    }
}
