using System;
using NUnit.Framework;
using System.Configuration;
using System.Linq;
using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess.Tests
{
    [TestFixture]
    public class AuthenticationTokenTests : TestBase
    {
        #region "Unit Tests"
        [Test]
        public void CanCreateToken()
        {
            var repository = new TokenRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var token = new AuthenticationToken
            {
                ContainerId = 1,
                CreatedBy = Guid.Parse("A85781A0-9194-44DF-91B3-F7F8274BF4A8"),
                CreatedOn = DateTime.Now,
                PrivateKey = Guid.Parse("{ACBE8427-B178-4D6A-A954-BC802E497EBB}"),
                PublicKey = Guid.Parse("{A85781A0-9194-44DF-91B3-F7F8274BF4A8}"),
                ValidFrom = DateTime.Now,
                Description = "New Authentication Token"
            };

            Assert.True(repository.CreateToken(token) != null);
        }

        [Test]
        public void CanGetToken()
        {
            var repository = new TokenRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var token = repository.GetToken(1);

            Assert.AreEqual(token.PublicKey, Guid.Parse("0904dc1e-8d2a-4d57-9fb2-248b7aa8a9a4"));
        }

        [Test]
        public void CanGetValidToken()
        {
            var repository = new TokenRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var token = repository.GetValidToken(Guid.Parse("24F35963-8ABF-4C11-AF35-BDA84819A04D"));

            Assert.AreEqual(token.PrivateKey, Guid.Parse("428b89d4-27b1-4af9-a0f9-f91c152e1e71"));
        }

        [Test]
        public void CanFailToGetGetInValidToken()
        {
            var repository = new TokenRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var token = repository.GetValidToken(Guid.Parse("82694346-58CE-4A1A-A9C7-26F590097A2A"));

            Assert.IsNull(token);
        }

        [Test]
        public void CanGetTokensForUser()
        {
            var repository = new TokenRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var tokens = repository.GetTokensForOwner(Guid.Parse("f833903d-3478-4d62-9bb7-960f5824b5e6"));

            Assert.IsTrue(tokens.Any());
        }

        [Test]
        public void CanUpdateToken()
        {
            var repository = new TokenRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var token = repository.GetToken(1);

            var newValidFrom = DateTime.Now;

            token.ValidFrom = newValidFrom;

            var result = repository.UpdateToken(token);

            Assert.IsTrue(result);

            var updatedToken = repository.GetToken(1);

            Assert.AreEqual(updatedToken.ValidFrom.DayOfWeek, newValidFrom.DayOfWeek);
        }

        [Test]
        public void CanDeleteToken()
        {
            var repository = new TokenRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var token = repository.GetToken(1);

            var result = repository.DeleteToken(token);

            Assert.IsTrue(result);

            var updatedToken = repository.GetToken(1);

            Assert.IsNotNull(updatedToken.ValidTo);
        }
        #endregion "Unit Tests"
    }
}
