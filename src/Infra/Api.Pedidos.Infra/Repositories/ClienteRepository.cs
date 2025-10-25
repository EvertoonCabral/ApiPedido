using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using Api.Pedidos.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Pedidos.Infra.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    private readonly  ProjectDbContext _context;
    public ClienteRepository(ProjectDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<Cliente?> GetByEmailAsync(string email, CancellationToken ct = default)
        => DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email, ct);

    public async Task<IReadOnlyList<Cliente>> ListarComPedidosAsync(CancellationToken ct = default) 
    {
            return await _context.Clientes
            .AsNoTracking()
            .Where(c => _context.Pedidos.Any(p => p.ClienteId == c.Id))
            .ToListAsync(ct);
            
    }
    }

