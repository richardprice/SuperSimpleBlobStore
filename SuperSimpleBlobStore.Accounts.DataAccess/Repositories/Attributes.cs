using System.Collections.Generic;
using System.Linq;

namespace SuperSimpleBlobStore.Accounts.DataAccess.Common
{
    public class Attributes<TContext> : IAttributes<TContext> where TContext : IDbContext
    {
        private readonly IUnitOfWork<TContext> _uow;

        public Attributes(IUnitOfWork<TContext> uow)
        {
            _uow = uow;
        }

        public List<IAttribute> GetAttributes(IEnumerable<IAttribute> source)
        {
            return (from a in source
                orderby a.SortOrder ascending
                select a).ToList<IAttribute>();
        }

        public List<TEntity> GetAttributes<TEntity>()
            where TEntity : class, IAttribute
        {
            return _uow.Context.Set<TEntity>().ToList();
        }

        public TEntity GetAttribute<TEntity>(int id)
            where TEntity : class, IAttribute
        {
            return _uow.Context.Set<TEntity>().Find(id);
        }

        public TEntity GetAttribute<TEntity>(string name)
    where TEntity : class, IAttribute
        {
            return _uow.Context.Set<TEntity>().FirstOrDefault(x => x.Name.Equals(name));
        }
    }
}
