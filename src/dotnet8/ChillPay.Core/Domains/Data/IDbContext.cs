using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChillPay.Core.Domains.Data;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry Attach(object entity);
    EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry Entry(object entity);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void Dispose();
}
