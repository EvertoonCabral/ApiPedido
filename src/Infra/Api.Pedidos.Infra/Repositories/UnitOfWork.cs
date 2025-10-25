using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Infra.Persistence;

namespace Api.Pedidos.Infra.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProjectDbContext _context;
    
    public UnitOfWork(ProjectDbContext context)
    {
        _context = context;
    }
    
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
      return  _context.SaveChangesAsync(ct);
    }
}