namespace EMed.Application.Interfaces;

public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}