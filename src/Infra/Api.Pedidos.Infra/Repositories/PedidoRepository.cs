using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using Api.Pedidos.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Pedidos.Infra.Repositories;

public class PedidoRepository : Repository<Pedido>, IPedidoRepository
{
    public PedidoRepository(ProjectDbContext context) : base(context) { }

    public Task<Pedido?> GetWithProdutosByIdAsync(int id, CancellationToken ct = default)
    {
        return DbSet
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }
    public async Task<Pedido?> GetWithItemAndProdutoAsync(int pedidoId, int produtoId, CancellationToken ct = default)
    {
        return await DbSet
            .Include(p => p.Itens.Where(i => i.ProdutoId == produtoId))
            .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.Id == pedidoId, ct);
    }
    public IQueryable<Pedido> Query() => DbSet.AsQueryable();
}