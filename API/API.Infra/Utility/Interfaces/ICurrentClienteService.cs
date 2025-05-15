namespace API.Infra.Utility.Interfaces
{
    public interface ICurrentClienteService
    {
        string? userId { get; }
        Guid? clienteId { get; }
    }
}
