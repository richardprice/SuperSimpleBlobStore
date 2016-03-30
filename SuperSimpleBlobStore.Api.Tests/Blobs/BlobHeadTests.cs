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
    public class BlobHeadTests
    {
        [Test]
        public void CanHeadBlob()
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

                with.Module<BlobHead>();
                with.Dependency(fakeProvider);
                with.ApplicationStartup((x, y) => StaticConfiguration.EnableHeadRouting = true);
            }));

            var result = browser.Head("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
            });

            Assert.IsTrue(result.Headers.ContainsKey("Etag"));
            Assert.AreEqual(result.Headers["Etag"], "E9606D05-786A-4CFF-B708-E4B747E3A452");
        }

        [Test]
        public void CanHeadBlobVersion()
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

                with.Module<BlobHead>();
                with.Dependency(fakeProvider);
                with.ApplicationStartup((x, y) => StaticConfiguration.EnableHeadRouting = true);
            }));

            var result = browser.Head("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
            });

            Assert.IsTrue(result.Headers.ContainsKey("Etag"));
            Assert.AreEqual(result.Headers["Etag"], "E9606D05-786A-4CFF-B708-E4B747E3A452");
        }
    }
}
