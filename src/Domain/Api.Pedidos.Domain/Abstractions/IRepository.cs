namespace Api.Pedidos.Domain.Abstractions;

public interface IRepository<T> : IReadOnlyRepository<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    void Update(T entity);
    Task DeleteByIdAsync(int id, CancellationToken ct = default);
}