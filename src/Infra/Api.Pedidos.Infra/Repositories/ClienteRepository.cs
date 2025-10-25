using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using Api.Pedidos.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Pedidos.Infra.Repositories;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    public ClienteRepository(ProjectDbContext context) : base(context) { }

    public Task<Cliente?> GetByEmailAsync(string email, CancellationToken ct = default)
        => _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Email == email, ct);
}