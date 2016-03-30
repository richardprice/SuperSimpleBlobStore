using System.Collections.Generic;

namespace SuperSimpleBlobStore.Accounts.DataAccess.Common
{
    public interface IAttributes
    {
        List<IAttribute> GetAttributes(IEnumerable<IAttribute> source);

        List<TEntity> GetAttributes<TEntity>()
            where TEntity : class, IAttribute;

        TEntity GetAttribute<TEntity>(int id)
            where TEntity : class, IAttribute;

        TEntity GetAttribute<TEntity>(string name)
            where TEntity : class, IAttribute;
    }

    public interface IAttributes<TContext>
    {
        List<IAttribute> GetAttributes(IEnumerable<IAttribute> source);

        List<TEntity> GetAttributes<TEntity>()
            where TEntity : class, IAttribute;

        TEntity GetAttribute<TEntity>(int id)
            where TEntity : class, IAttribute;

        TEntity GetAttribute<TEntity>(string name)
            where TEntity : class, IAttribute;
    }
}