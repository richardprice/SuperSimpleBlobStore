using System;
using NUnit.Framework;
using System.Configuration;
using System.IO;
using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess.Tests
{
    [TestFixture]
    public class BlobTests : TestBase
    {
        #region "Unit Tests"
        [Test]
        public void CanCreateBlobDetails()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = new BlobDetails
            {
                BlobId = Guid.NewGuid(),
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                LastModified = DateTime.Now,
                LastModifiedBy = Guid.NewGuid(),
                ValidFrom = DateTime.Now,
                ContainerId = 1
            };

            Assert.True(repository.CreateBlobDetails(blob) != null);
        }

        [Test]
        public void CanGetBlobDetails()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobDetails(1);

            Assert.AreEqual(blob.ContainerId, 1);
        }

        [Test]
        public void CanUpdateBlobDetails()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobDetails(1);

            Assert.AreEqual(blob.ContainerId, 1);

            blob.LastModified = DateTime.Now;

            Assert.True(repository.UpdateBlobDetails(blob));
        }

        [Test]
        public void CanDeleteBlobDetails()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobDetails(1);

            Assert.AreEqual(blob.ContainerId, 1);

            Assert.True(repository.DeleteBlobDetails(blob));
        }

        [Test]
        public void CanCreateBlobVersion()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = new BlobVersion
            {
                VersionId = Guid.NewGuid(),
                BlobContentsId = 1,
                BlobId = 1,
                ContentLength = 10,
                ContentType = "image/jpeg",
                FileName = "test.jpg",
                BlobVariantHash = String.Empty,
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                LastModified = DateTime.Now,
                LastModifiedBy = Guid.NewGuid(),
                ValidFrom = DateTime.Now,
                Etag = Guid.NewGuid().ToString()
            };

            Assert.True(repository.CreateBlobVersion(blob) != null);
        }

        [Test]
        public void CanGetBlobVersion()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobVersion(1);

            Assert.AreEqual(blob.FileName, "test.jpg");
        }

        [Test]
        public void CanUpdateBlobVersion()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobVersion(1);

            Assert.AreEqual(blob.FileName, "test.jpg");

            blob.LastModified = DateTime.Now;

            Assert.True(repository.UpdateBlobVersion(blob));
        }

        [Test]
        public void CanDeleteBlobVersion()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobVersion(1);

            Assert.AreEqual(blob.FileName, "test.jpg");

            Assert.True(repository.DeleteBlobVersion(blob));
        }

        [Test]
        public void CanCreateBlobContents()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var location = new Uri(LocalPath);
            var contents = File.ReadAllBytes(Path.Combine(location.LocalPath, "test.jpg"));

            var blob = new BlobContents
            {
                Content = contents
            };

            Assert.True(repository.CreateBlobContents(blob) != null);
        }

        [Test]
        public void CanGetBlobContents()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobContents(1);

            Assert.IsTrue(blob.Content.Length > 0);
        }

        [Test]
        public void CanGetBlobAggregate()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetLatestValidBlob(Guid.Parse("61A2779F-FB7B-4129-8878-9C2D3647E717"));

            Assert.AreEqual(blob.ContainerIdentity, Guid.Parse("F833903D-3478-4D62-9BB7-960F5824B5E6"));
        }

        [Test]
        public void CanGetBlobAggregateContents()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetLatestValidBlobContents(Guid.Parse("61A2779F-FB7B-4129-8878-9C2D3647E717"));

            Assert.IsTrue(blob.Length > 0);
        }

        [Test]
        public void CanGetBlobForDate()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobForDate(Guid.Parse("61A2779F-FB7B-4129-8878-9C2D3647E717"), new DateTime(2016, 01, 25));

            Assert.AreEqual(Guid.Parse("37a69b95-429d-41bb-a10c-91156f63f9ff"), blob.VersionId);
        }

        [Test]
        public void CanGetBlobForSecondDate()
        {
            var repository = new BlobRepository("Database=unit_" + DbName + ";" + ConfigurationProvider.BlobStoreDatabaseConnectionString);

            var blob = repository.GetBlobForDate(Guid.Parse("61A2779F-FB7B-4129-8878-9C2D3647E717"), new DateTime(2016, 02, 25));

            Assert.AreEqual(Guid.Parse("855CB43E-D5A2-4710-B2D5-6E6FDB59DA2A"), blob.VersionId);
        }
        #endregion "Unit Tests"
    }
}
