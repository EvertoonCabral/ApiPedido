using Api.Pedidos.Domain.Repositories;
using Api.Pedidos.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Pedidos.Infra.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(DbContext context) : base(context) { }

    public async Task<IReadOnlyList<Produto>> ListAtivosAsync(CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Where(p => p.IsAtivo)
            .ToListAsync(ct);
}