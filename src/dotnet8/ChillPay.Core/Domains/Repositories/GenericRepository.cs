using ChillPay.Core.Domains.Data;
using ChillPay.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChillPay.Core.Domains.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly IDbContext _context;
    protected bool _disposed;

    protected GenericRepository(IDbContext context, DbSet<TEntity> dbSet)
    {
        _context = context;
        _dbSet = dbSet;
    }

    protected DatabaseFacade Database => ((DbContext)_context).Database;

    public virtual async Task<long> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public virtual void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        _dbSet.Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        _dbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await _context.SaveChangesAsync(cancellationToken);
        if (result < 0)
        {
            throw new InvalidOperationException("Cannot save changes in db.");
        }

        return result;
    }

    public void SetPropertyIsModified(TEntity entity, string propertyName)
    {
        _context.Entry(entity).Property(propertyName).IsModified = true;
    }

    protected virtual async Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual IQueryable<TEntity> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }
}
