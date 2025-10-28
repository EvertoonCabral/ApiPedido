using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;

namespace Api.Pedidos.Domain.Repositories;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<Pedido?> GetWithProdutosByIdAsync(int id, CancellationToken ct = default);
    Task<Pedido?> GetWithItemAndProdutoAsync(int pedidoId, int produtoId, CancellationToken ct = default);
    IQueryable<Pedido> Query();
}