using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;

namespace Api.Pedidos.Domain.Repositories;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<Pedido?> GetWithProdutosByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Pedido>> ListByClienteAsync (int clienteId, CancellationToken ct = default);
}