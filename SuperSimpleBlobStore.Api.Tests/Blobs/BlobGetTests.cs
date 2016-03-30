using System;
using System.IO;
using System.Reflection;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using SuperSimpleBlobStore.Api.Modules.Blobs;
using SuperSimpleBlobStore.Api.ViewModel.Blobs;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Tests.Blobs
{
    [TestFixture]
    public class BlobGetTests
    {
        [Test]
        public void CanGetBlob()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();

                var location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));

                A.CallTo(() => fakeProvider.GetBlob(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(new Blob
                    {
                        Etag = "E9606D05-786A-4CFF-B708-E4B747E3A452",
                        VersionLastModified = DateTime.Now,
                        ContentType = "image/jpeg",
                        ContentLength = 10,
                        BlobContentsId = 1
                    });

                A.CallTo(() => fakeProvider.GetContents(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(File.ReadAllBytes(Path.Combine(location.LocalPath, "test.jpg")));

                with.Module<BlobGet>();
                with.Dependency(fakeProvider);

            }));

            var result = browser.Get("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
            });

            Assert.IsTrue(result.Headers.ContainsKey("Etag"));
            Assert.AreEqual(result.Headers["Etag"], "E9606D05-786A-4CFF-B708-E4B747E3A452");
            Assert.IsTrue(result.Body.AsStream().Length > 0);
        }

        [Test]
        public void CanGetBlobForDownload()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();

                var location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));

                A.CallTo(() => fakeProvider.GetBlob(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(new Blob
                    {
                        Etag = "E9606D05-786A-4CFF-B708-E4B747E3A452",
                        VersionLastModified = DateTime.Now,
                        ContentType = "image/jpeg",
                        ContentLength = 10,
                        BlobContentsId = 1
                    });

                A.CallTo(() => fakeProvider.GetContents(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(File.ReadAllBytes(Path.Combine(location.LocalPath, "test.jpg")));

                with.Module<BlobGet>();
                with.Dependency(fakeProvider);

            }));

            var result = browser.Get("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
                with.Query("dl", "1");
            });

            Assert.IsTrue(result.Headers.ContainsKey("Etag"));
            Assert.AreEqual(result.Headers["Etag"], "E9606D05-786A-4CFF-B708-E4B747E3A452");
            Assert.IsTrue(result.Headers.ContainsKey("Content-Disposition"));
            Assert.IsTrue(result.Headers["Content-Disposition"].Contains("attachment"));
            Assert.IsTrue(result.Body.AsStream().Length > 0);
        }

        [Test]
        public void CanGetBlobVersion()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();

                var location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));

                A.CallTo(() => fakeProvider.GetBlobVersion(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(new Blob
                    {
                        Etag = "E9606D05-786A-4CFF-B708-E4B747E3A452",
                        VersionLastModified = DateTime.Now,
                        ContentType = "image/jpeg",
                        ContentLength = 10,
                        BlobContentsId = 1
                    });

                A.CallTo(() => fakeProvider.GetContentsForVersion(A<Guid>._, A<Guid>._))
                    .Returns(File.ReadAllBytes(Path.Combine(location.LocalPath, "test.jpg")));

                with.Module<BlobGet>();
                with.Dependency(fakeProvider);

            }));

            var result = browser.Get("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
            });

            Assert.IsTrue(result.Headers.ContainsKey("Etag"));
            Assert.AreEqual(result.Headers["Etag"], "E9606D05-786A-4CFF-B708-E4B747E3A452");
            Assert.IsTrue(result.Body.AsString().Length > 0);
        }
    }
}
