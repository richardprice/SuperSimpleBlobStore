using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.Accounts.DataAccess.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public class UserAccountContext : DbContext, IDbContext
    {
        static UserAccountContext()
        {
            Database.SetInitializer(DatabaseInitilizerFactory.GetInitializer());
        }

        public UserAccountContext()
        {
            Database.Connection.ConnectionString =
                ConfigurationProvider.UserAccountsDatabaseConnectionString;
        }

        public static UserAccountContext GetContext()
        {
            return new UserAccountContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer(DatabaseInitilizerFactory.GetInitializer());
        }

        // The Model
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }
        public DbSet<TrackingEvent> TrackingEvents { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
        }
    }
}
