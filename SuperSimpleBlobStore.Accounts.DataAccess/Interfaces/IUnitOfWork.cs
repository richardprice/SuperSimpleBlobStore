using System.Data.Entity;

namespace SuperSimpleBlobStore.Accounts.DataAccess.Common
{
    public interface IUnitOfWork<TContext> where TContext : IDbContext
    {
        IDbContext Context { get; set; }
        int Save();
    }
}
