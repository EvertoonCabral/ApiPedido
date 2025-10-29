using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;

namespace Api.Pedidos.Domain.Repositories;

public interface IClienteRepository : IRepository<Cliente>
{
    Task<IReadOnlyList<Cliente>> ListarComPedidosAsync(CancellationToken ct = default);
    Task<Cliente?> GetByEmailAsync(string email, CancellationToken ct);
    
}