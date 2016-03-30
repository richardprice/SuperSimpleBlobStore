using System;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using System.Configuration;
using System.Linq;
using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess.Tests
{
    [TestFixture]
    public class ContainerTests : TestBase
    {
        #region "Unit Tests"
        [Test]
        public void CanCreateContainer()
        {
            var repository = new ContainerRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var container = new Container
            {
                ContainerIdentity = Guid.NewGuid(),
                Name = "Container Creation Test",
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                Etag = Guid.NewGuid().ToString(),
                LastModified = DateTime.Now,
                LastModifiedBy = Guid.NewGuid(),
                ValidFrom = DateTime.Now
            };

            Assert.True(repository.CreateContainer(container) != null);
        }

        [Test]
        public void CanGetContainer()
        {
            var repository = new ContainerRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var container = repository.GetContainer(1);

            Assert.AreEqual(container.ContainerIdentity, Guid.Parse("f833903d-3478-4d62-9bb7-960f5824b5e6"));
        }

        [Test]
        public void CanGetValidContainer()
        {
            var repository = new ContainerRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var container = repository.GetValidContainer(Guid.Parse("{DD68D28B-9643-4524-B947-10DF85023F76}"));

            Assert.IsNotNull(container);
        }

        [Test]
        public void CanNotGetInValidContainer()
        {
            var repository = new ContainerRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var container = repository.GetValidContainer(Guid.Parse("{8B273983-A8E0-4BAC-B0DE-D3E30B1B94A1}"));

            Assert.IsNull(container);
        }

        [Test]
        public void CanGetContainersForUser()
        {
            var repository = new ContainerRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var containers = repository.GetContainersForOwner(Guid.Parse("f833903d-3478-4d62-9bb7-960f5824b5e6"));

            Assert.IsTrue(containers.Any());
        }

        [Test]
        public void CanUpdateContainer()
        {
            var repository = new ContainerRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var container = repository.GetContainer(1);

            container.Name = "Updated Container";

            var result = repository.UpdateContainer(container);

            Assert.IsTrue(result);

            var updatedContainer = repository.GetContainer(1);

            Assert.AreEqual(updatedContainer.Name, "Updated Container");
        }

        [Test]
        public void CanDeleteContainer()
        {
            var repository = new ContainerRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var container = repository.GetContainer(1);

            var result = repository.DeleteContainer(container);

            Assert.IsTrue(result);

            var updatedContainer = repository.GetContainer(1);

            Assert.IsNotNull(updatedContainer.ValidTo);
        }
        #endregion "Unit Tests"
    }
}
