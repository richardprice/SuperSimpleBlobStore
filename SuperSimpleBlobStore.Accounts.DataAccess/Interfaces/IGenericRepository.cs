using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSimpleBlobStore.Accounts.DataAccess.Common
{
    public interface IGenericRepository<TEntity>
     where TEntity : class, IEntity
    {
        void AddOrUpdate(TEntity entity);
        IList<TEntity> GetAll();
        IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(int id);
        TEntity NewEntity();
        TObject CreateObject<TObject>() where TObject : class;
        IQueryable<TEntity> Query();
    }

    public interface IGenericRepository<TContext, TEntity>
        where TEntity : class, IEntity
        where TContext : IDbContext
    {
        void AddOrUpdate(TEntity entity);
        IList<TEntity> GetAll();
        IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
        TEntity GetById(int id);
        TEntity NewEntity();
        TObject CreateObject<TObject>() where TObject : class;
        IQueryable<TEntity> Query();
    }
}
