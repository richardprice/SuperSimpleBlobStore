using System;
using System.Data.Entity;
using System.Linq;
using SuperSimpleBlobStore.Accounts.DataAccess.Common;
using SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts;
using SuperSimpleBlobStore.Common;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public class DropCreateDatabaseWithSeedData : DropCreateDatabaseIfModelChanges<UserAccountContext>
    {
        protected override void Seed(UserAccountContext context)
        {
            var i = 1;
            foreach (Claims claim in Enum.GetValues(typeof(Claims)))
            {
                context.Claims.Add(new Claim { Id = i, Name = claim.ToString(), SortOrder = i });
                i++;
            }

            context.SaveChanges();

            context.Credentials.Add(new Credential
            {
                Id = 1,
                EmailAddress = "example@example.com",
                Password = "ADSJBawFLKA1U0DaqN38YSHuBuvjClv7PqnDs/MTpU0yzgbYb4tlpA8dHcdeQEmEAQ==",
                ValidFrom = DateTime.Now,
                EncryptionScheme = CryptoSchemes.Rfc289
            });

            context.Credentials.Add(new Credential
            {
                Id = 2,
                EmailAddress = "example2@example.com",
                Password = "ADSJBawFLKA1U0DaqN38YSHuBuvjClv7PqnDs/MTpU0yzgbYb4tlpA8dHcdeQEmEAQ==",
                ValidFrom = DateTime.Now,
                EncryptionScheme = CryptoSchemes.Rfc289
            });
            
            context.Credentials.Add(new Credential
            {
                Id = 3,
                EmailAddress = "example3@example.com",
                Password = "ADSJBawFLKA1U0DaqN38YSHuBuvjClv7PqnDs/MTpU0yzgbYb4tlpA8dHcdeQEmEAQ==",
                ValidFrom = DateTime.Now,
                EncryptionScheme = CryptoSchemes.Rfc289
            });
            
            context.Credentials.Add(new Credential
            {
                Id = 4,
                EmailAddress = "example4@example.com",
                Password = "ADSJBawFLKA1U0DaqN38YSHuBuvjClv7PqnDs/MTpU0yzgbYb4tlpA8dHcdeQEmEAQ==",
                ValidFrom = DateTime.Now,
                EncryptionScheme = CryptoSchemes.Rfc289
            });
            
            context.Credentials.Add(new Credential
            {
                Id = 5,
                EmailAddress = "example5@example.com",
                Password = "ADSJBawFLKA1U0DaqN38YSHuBuvjClv7PqnDs/MTpU0yzgbYb4tlpA8dHcdeQEmEAQ==",
                ValidFrom = DateTime.Now,
                EncryptionScheme = CryptoSchemes.Rfc289
            });

            context.SaveChanges();

            context.People.Add(new Person
            {
                Id = 1,
                AuthenticationId = Guid.NewGuid(),
                Name = "Debug user 1",
                ValidFrom = DateTime.Now,
                LastUpdated = DateTime.Now,
                Claims = { context.Claims.First(x => x.Id == 1), context.Claims.First(x => x.Id == 2), context.Claims.First(x => x.Id == 3), context.Claims.First(x => x.Id == 4) },
                Credentials = { context.Credentials.First(x => x.Id == 1) }
            });

            context.People.Add(new Person
            {
                Id = 2,
                AuthenticationId = Guid.NewGuid(),
                Name = "Debug user 2",
                ValidFrom = DateTime.Now,
                LastUpdated = DateTime.Now,
                Claims = { context.Claims.First(x => x.Id == 1), context.Claims.First(x => x.Id == 2), context.Claims.First(x => x.Id == 3), context.Claims.First(x => x.Id == 4) },
                Credentials = { context.Credentials.First(x => x.Id == 2) }
            });

            context.People.Add(new Person
            {
                Id = 3,
                AuthenticationId = Guid.NewGuid(),
                Name = "Debug user 3",
                ValidFrom = DateTime.Now,
                LastUpdated = DateTime.Now,
                Claims = { context.Claims.First(x => x.Id == 1), context.Claims.First(x => x.Id == 2), context.Claims.First(x => x.Id == 3), context.Claims.First(x => x.Id == 4) },
                Credentials = { context.Credentials.First(x => x.Id == 3) }
            });

            context.People.Add(new Person
            {
                Id = 4,
                AuthenticationId = Guid.NewGuid(),
                Name = "Debug user 4",
                ValidFrom = DateTime.Now,
                LastUpdated = DateTime.Now,
                Claims = { context.Claims.First(x => x.Id == 1), context.Claims.First(x => x.Id == 2), context.Claims.First(x => x.Id == 3), context.Claims.First(x => x.Id == 4) },
                Credentials = { context.Credentials.First(x => x.Id == 4) }
            });

            context.People.Add(new Person
            {
                Id = 5,
                AuthenticationId = Guid.NewGuid(),
                Name = "Debug user 5",
                ValidFrom = DateTime.Now,
                LastUpdated = DateTime.Now,
                Claims = { context.Claims.First(x => x.Id == 1), context.Claims.First(x => x.Id == 2), context.Claims.First(x => x.Id == 3), context.Claims.First(x => x.Id == 4) },
                Credentials = { context.Credentials.First(x => x.Id == 5) }
            });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
