using System;
using System.Data.Entity;
using System.Linq;
using SuperSimpleBlobStore.Accounts.DataAccess.Common;
using SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts;
using SuperSimpleBlobStore.Common;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public class DropCreateDatabaseWithIntegrationData : DropCreateDatabaseAlways<UserAccountContext>
    {
        protected override void Seed(UserAccountContext context)
        {
            var i = 1;
            foreach (Claims claim in Enum.GetValues(typeof (Claims)))
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
                EmailAddress = "legacyexample@example.com",
                Password = "CGHY2EKayFQgQmFhSeG+Dw==",
                ValidFrom = DateTime.Now,
                EncryptionScheme = CryptoSchemes.LegacyLandlordsQuote
            });

            context.SaveChanges();

            context.VerificationTokens.Add(new VerificationToken
            {
                Id = 1,
                Reason = TokenUses.AccountVerification,
                Token = Guid.Parse("15e13c81-0bf8-43cd-b664-631ca48f7531"),
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddDays(1)
            });

            context.SaveChanges();

            context.People.Add(new Person
            {
                Id = 1,
                AuthenticationId = Guid.Parse("2af7485d-a0bd-4be7-b671-79c6eabf47f4"),
                ValidFrom = DateTime.Now,
                LastUpdated = DateTime.Now,
                Name = "Integration Data User",
                Claims = { context.Claims.First(x => x.Id == 1), context.Claims.First(x => x.Id == 2) },
                Credentials = { context.Credentials.First(x => x.Id == 1) },
                VerificationTokens = {  context.VerificationTokens.First(x => x.Id == 1)}
            });

            context.People.Add(new Person
            {
                Id = 2,
                AuthenticationId = Guid.Parse("F3751E1F-61EF-4ECF-99DE-C695BC09ED1F"),
                ValidFrom = DateTime.Now,
                LastUpdated = DateTime.Now,
                Name = "Integration Data Legacy User",
                Claims = { context.Claims.First(x => x.Id == 1), context.Claims.First(x => x.Id == 2) },
                Credentials = { context.Credentials.First(x => x.Id == 2) }
            });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
