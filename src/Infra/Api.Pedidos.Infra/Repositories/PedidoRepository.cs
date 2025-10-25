using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Pedidos.Infra.Repositories;

public class PedidoRepository : Repository<Pedido>, IPedidosRepository
{
    public PedidoRepository(DbContext context) : base(context) { }
    
    public Task<Pedido?> GetWithProdutosByIdAsync(int id, CancellationToken ct = default)
        => _dbSet
            .Include(p => p.Produtos)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);


    public async Task<IReadOnlyList<Pedido>> ListByClienteAsync(int clienteId, CancellationToken ct = default)
        => await _dbSet.AsNoTracking()
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync(ct);
    
}