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

    public async Task<IReadOnlyList<Pedido>> ListByClienteAsync(int clienteId, CancellationToken ct = default)
    {
      return  await DbSet.AsNoTracking()
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync(ct);
    }

    public IQueryable<Pedido> Query() => DbSet.AsQueryable();
}