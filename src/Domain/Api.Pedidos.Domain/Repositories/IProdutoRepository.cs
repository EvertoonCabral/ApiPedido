using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;

namespace Api.Pedidos.Domain.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IReadOnlyList<Produto>> ListAtivosAsync(CancellationToken ct = default);
}