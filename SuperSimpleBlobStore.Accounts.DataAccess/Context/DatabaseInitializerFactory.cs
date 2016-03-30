using System.Data.Entity;
using SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts;
using SuperSimpleBlobStore.Common;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public static class DatabaseInitilizerFactory
    {
        public static IDatabaseInitializer<UserAccountContext> GetInitializer()
        {
            switch (SuperSimpleBlobStore.Common.ConfigurationProvider.EnvironmentType)
            {
                case ApplicationEnvironment.Debug:
                    return new DropCreateDatabaseWithSeedData();
                case ApplicationEnvironment.Integration:
                    return new DropCreateDatabaseWithIntegrationData();
                case ApplicationEnvironment.UAT:
                    return null;
                case ApplicationEnvironment.Release:
                    return null;
                default:
                    return null;
            }
        }
    }
}
