using ChillPay.Core.Domains.Entities;

namespace ChillPay.Core.Domains.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<long> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);

    IQueryable<TEntity> GetQueryable();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void SetPropertyIsModified(TEntity entity, string propertyName);
}
