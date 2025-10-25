using Api.Pedidos.Domain.Repositories;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Pedidos.Infra.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(ProjectDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Produto>> ListAtivosAsync(CancellationToken ct = default)
    {
        return await DbSet.AsNoTracking()
            .Where(p => p.IsAtivo)
            .ToListAsync(ct);
    }
}