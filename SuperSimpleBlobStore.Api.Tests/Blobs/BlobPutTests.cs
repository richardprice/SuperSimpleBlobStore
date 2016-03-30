using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using FakeItEasy;
using Nancy;
using Nancy.Owin;
using Nancy.Testing;
using NUnit.Framework;
using SuperSimpleBlobStore.Api.Modules.Blobs;
using SuperSimpleBlobStore.Api.Tests.Infrastructure;
using SuperSimpleBlobStore.Api.ViewModel.Blobs;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Tests.Blobs
{
    [TestFixture]
    public class BlobPutTests : ApiTestBase
    {
        [Test]
        public void CanPutBlobForCreation()
        {
            var location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));

            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();
                var fakeTokensProvider = A.Fake<ITokens>();

                A.CallTo(() => fakeProvider.CreateBlobVersion(A<Guid>._, A<byte[]>._, A<string>._, A<string>._, A<string>._, A<Guid>._))
                    .Returns(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"));

                A.CallTo(() => fakeTokensProvider.CanAccessContainer(A<Guid>._, A<Guid>._))
                    .Returns(true);

                with.Module<BlobPut>();
                with.Dependency(fakeProvider);
                with.Dependency(fakeTokensProvider);

                with.RequestStartup((container, pipelines, context) =>
                {
                    context.Items[NancyMiddleware.RequestEnvironmentKey] = OwinUserInformation;
                });
            }));

            var stream = File.OpenRead(Path.Combine(location.LocalPath, "test.jpg"));

            var multipart = new BrowserContextMultipartFormData(with => {
                with.AddFile("BlobContent", "test.jpg", "image/jpeg", stream);
                with.AddFormField("BlobId", "text/plain", Guid.NewGuid().ToString());
                with.AddFormField("FileName", "text/plain", "test.jpg");
                with.AddFormField("ContentType", "text/plain", "image/jpeg");
                with.AddFormField("Etag", "text/plain", Guid.NewGuid().ToString());
            });

            var result = browser.Put("/blob/", with =>
            {
                with.HttpRequest();
                with.MultiPartFormData(multipart);
            });

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.IsTrue(result.Body.AsString().ToLowerInvariant().Contains("versionidentity"));
        }

        [Test]
        public void CanNotPutBlobForCreationAsUnauthenticatedUser()
        {
            var location = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase));

            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();
                var fakeTokensProvider = A.Fake<ITokens>();

                A.CallTo(() => fakeProvider.CreateBlobVersion(A<Guid>._, A<byte[]>._, A<string>._, A<string>._, A<string>._, A<Guid>._))
                    .Returns(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"));

                A.CallTo(() => fakeTokensProvider.CanAccessContainer(A<Guid>._, A<Guid>._))
                    .Returns(true);

                with.Module<BlobPut>();
                with.Dependency(fakeProvider);
                with.Dependency(fakeTokensProvider);
            }));

            var stream = File.OpenRead(Path.Combine(location.LocalPath, "test.jpg"));

            var multipart = new BrowserContextMultipartFormData(with => {
                with.AddFile("BlobContent", "test.jpg", "image/jpeg", stream);
                with.AddFormField("BlobId", "text/plain", Guid.NewGuid().ToString());
                with.AddFormField("FileName", "text/plain", "test.jpg");
                with.AddFormField("ContentType", "text/plain", "image/jpeg");
                with.AddFormField("Etag", "text/plain", Guid.NewGuid().ToString());
            });

            var result = browser.Put("/blob/", with =>
            {
                with.HttpRequest();
                with.MultiPartFormData(multipart);
            });

            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }
}
