using System;
using System.Data.Entity;

namespace SuperSimpleBlobStore.Accounts.DataAccess.Common
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDbContext
    {
        private IDbContext _context;

        public UnitOfWork()
        {
            _context = (TContext)Activator.CreateInstance(typeof (TContext));
        }

        public IDbContext Context
        {
            get
            {
                return _context;
            }

            set
            {
                _context = value;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
