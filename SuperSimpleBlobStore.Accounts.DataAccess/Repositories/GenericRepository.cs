using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace SuperSimpleBlobStore.Accounts.DataAccess.Common
{
    public class GenericRepository<TContext, TEntity> : IGenericRepository<TContext, TEntity> 
        where TEntity : class, IEntity 
        where TContext : IDbContext
    {
        private readonly IDbContext _context;

        public GenericRepository(IUnitOfWork<TContext> uow)
        {
            _context = uow.Context;
        }

        public TEntity NewEntity()
        {
            var t = _context.Set<TEntity>().Create();
            _context.Set<TEntity>().Add(t);
            return t;
        }

        public void AddOrUpdate(TEntity entity)
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                var t = typeof(TEntity);
                Debug.WriteLine("Generic Repository exception on AddOrUpdate for type " + t + " :: " + ex);
                throw;
            }
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public IList<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ToList();
        }

        public IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public TObject CreateObject<TObject>()
            where TObject : class
        {
            return _context.Set<TObject>().Create();
        }

        public void RefreshObject<TObject>(TObject instance)
            where TObject : class
        {
            _context.Entry(instance).Reload();
        }
    }
}
