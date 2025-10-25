using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;

namespace Api.Pedidos.Domain.Repositories;

public interface IProdutoRepository : IRepository<Produtos>
{
    Task<IReadOnlyList<Produtos>> ListAtivosAsync(CancellationToken ct = default);
}