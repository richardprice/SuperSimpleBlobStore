using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using SuperSimpleBlobStore.DataAccess;
using SuperSimpleBlobStore.DataAccess.Model;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Domain.Tests.ProviderTests
{
    [TestFixture]
    public class BlobProviderTests
    {
        [Test]
        public void CanCreateBlob()
        {
            var fakeBlobRepository = A.Fake<IBlobRepository>();
            var fakeContainerRepository = A.Fake<IContainerRepository>();
            var location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));

            A.CallTo(() => fakeContainerRepository.GetContainer(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                .Returns(new Container
                {
                    Id = 1,
                    Name = "FakeItEasy Container",
                    ContainerIdentity = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    LastModified = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    LastModifiedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")
                });

            A.CallTo(() => fakeBlobRepository.CreateBlobContents(A<BlobContents>._))
                .WithAnyArguments()
                .ReturnsLazily((BlobContents contents) => new BlobContents
                {
                    Id = 1,
                    Content = contents.Content
                });

            A.CallTo(() => fakeBlobRepository.CreateBlobDetails(A<BlobDetails>._))
                .WithAnyArguments()
                .ReturnsLazily((BlobDetails details) => new BlobDetails
                {
                    Id = 1,
                    BlobId = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    ContainerId = details.ContainerId,
                    CreatedOn = details.CreatedOn,
                    CreatedBy = details.CreatedBy,
                    LastModifiedBy = details.LastModifiedBy,
                    LastModified = details.LastModified,
                    ValidFrom = details.ValidFrom
                });

            A.CallTo(() => fakeBlobRepository.CreateBlobVersion(A<BlobVersion>._))
                .WithAnyArguments()
                .ReturnsLazily((BlobVersion version) => new BlobVersion
                {
                    Id = 1,
                    BlobContentsId = version.BlobContentsId,
                    BlobId = version.BlobId,
                    BlobVariantHash = version.BlobVariantHash,
                    ContentLength = version.ContentLength,
                    ContentType = version.ContentType,
                    CreatedBy = version.CreatedBy,
                    CreatedOn = version.CreatedOn,
                    Etag = version.Etag,
                    FileName = version.FileName,
                    VersionId = version.VersionId,
                    LastModifiedBy = version.LastModifiedBy,
                    LastModified = version.LastModified,
                    ValidFrom = version.ValidFrom
                });

            var containerProvider = new Blobs(fakeBlobRepository, fakeContainerRepository);

            var result = containerProvider.CreateBlob(
                File.ReadAllBytes(Path.Combine(location.LocalPath, "test.jpg")),
                "test.jpg",
                Guid.NewGuid().ToString(),
                "image/jpeg",
                Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"));

            Assert.AreEqual(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), result);
        }

        [Test]
        public void CanCreateBlobVersion()
        {
            var fakeBlobRepository = A.Fake<IBlobRepository>();
            var fakeContainerRepository = A.Fake<IContainerRepository>();
            var location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));

            A.CallTo(() => fakeBlobRepository.CreateBlobContents(A<BlobContents>._))
                .WithAnyArguments()
                .ReturnsLazily((BlobContents contents) => new BlobContents
                {
                    Id = 2,
                    Content = contents.Content
                });

            A.CallTo(() => fakeBlobRepository.GetBlobDetails(A<Guid>._))
                .WithAnyArguments()
                .ReturnsLazily((Guid id) => new BlobDetails
                {
                    Id = 1,
                    BlobId = id,
                    ContainerId = 1,
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    LastModifiedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    LastModified = DateTime.Now,
                    ValidFrom = DateTime.Now
                });

            A.CallTo(() => fakeBlobRepository.CreateBlobVersion(A<BlobVersion>._))
                .WithAnyArguments()
                .ReturnsLazily((BlobVersion version) => new BlobVersion
                {
                    Id = 1,
                    BlobContentsId = version.BlobContentsId,
                    BlobId = version.BlobId,
                    BlobVariantHash = version.BlobVariantHash,
                    ContentLength = version.ContentLength,
                    ContentType = version.ContentType,
                    CreatedBy = version.CreatedBy,
                    CreatedOn = version.CreatedOn,
                    Etag = version.Etag,
                    FileName = version.FileName,
                    VersionId = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    LastModifiedBy = version.LastModifiedBy,
                    LastModified = version.LastModified,
                    ValidFrom = version.ValidFrom
                });

            var containerProvider = new Blobs(fakeBlobRepository, fakeContainerRepository);

            var result = containerProvider.CreateBlobVersion(
                Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                File.ReadAllBytes(Path.Combine(location.LocalPath, "test.jpg")),
                "test.jpg",
                Guid.NewGuid().ToString(),
                "image/jpeg",
                Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"));

            Assert.AreEqual(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), result);
        }

        [Test]
        public void CanGetBlob()
        {
            var fakeBlobRepository = A.Fake<IBlobRepository>();
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeBlobRepository.GetLatestValidBlob(A<Guid>._))
                .WithAnyArguments()
                .ReturnsLazily((Guid id) => new Blob
                {
                    BlobId = id,
                });

            var blobsProvider = new Blobs(fakeBlobRepository, fakeContainerRepository);

            var result = blobsProvider.GetBlob(
                Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"));

            Assert.AreEqual(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), result.BlobId);
        }

        [Test]
        public void CanGetBlobForDate()
        {
            var fakeBlobRepository = A.Fake<IBlobRepository>();
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeBlobRepository.GetBlobForDate(A<Guid>._, A<DateTime>._))
                .WithAnyArguments()
                .ReturnsLazily((Guid id, DateTime date) => new Blob
                {
                    BlobId = id,
                    VersionId = id
                });

            var blobsProvider = new Blobs(fakeBlobRepository, fakeContainerRepository);

            var result = blobsProvider.GetBlobForDate(
                Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), DateTime.Now);

            Assert.AreEqual(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), result.VersionId);
        }
    }
}
