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
    public class BlobDeleteTests : ApiTestBase
    {
        [Test]
        public void CanDeleteBlob()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();
                var fakeTokensProvider = A.Fake<ITokens>();

                A.CallTo(() => fakeProvider.DeleteBlob(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(true);

                A.CallTo(() => fakeTokensProvider.CanAccessContainer(A<Guid>._, A<Guid>._))
                    .Returns(true);

                with.Module<BlobDelete>();
                with.Dependency(fakeProvider);
                with.Dependency(fakeTokensProvider);

                with.RequestStartup((container, pipelines, context) =>
                {
                    context.Items[NancyMiddleware.RequestEnvironmentKey] = OwinUserInformation;
                });
            }));

            var result = browser.Delete("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
            });
            
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void CanDeleteBlobVersion()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();
                var fakeTokensProvider = A.Fake<ITokens>();

                A.CallTo(() => fakeProvider.DeleteBlobVersion(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(true);

                A.CallTo(() => fakeTokensProvider.CanAccessContainer(A<Guid>._, A<Guid>._))
                    .Returns(true);

                with.Module<BlobDelete>();
                with.Dependency(fakeProvider);
                with.Dependency(fakeTokensProvider);

                with.RequestStartup((container, pipelines, context) =>
                {
                    context.Items[NancyMiddleware.RequestEnvironmentKey] = OwinUserInformation;
                });
            }));

            var result = browser.Delete("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
            });

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void CanNotDeleteBlobAsUnauthenticatedUser()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();
                var fakeTokensProvider = A.Fake<ITokens>();

                A.CallTo(() => fakeProvider.DeleteBlob(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(true);

                A.CallTo(() => fakeTokensProvider.CanAccessContainer(A<Guid>._, A<Guid>._))
                    .Returns(true);

                with.Module<BlobDelete>();
                with.Dependency(fakeProvider);
                with.Dependency(fakeTokensProvider);

            }));

            var result = browser.Delete("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
            });

            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Test]
        public void CanNotDeleteBlobVersionAsUnauthenticatedUser()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeProvider = A.Fake<IBlobs>();
                var fakeTokensProvider = A.Fake<ITokens>();

                A.CallTo(() => fakeProvider.DeleteBlobVersion(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                    .Returns(true);

                A.CallTo(() => fakeTokensProvider.CanAccessContainer(A<Guid>._, A<Guid>._))
                    .Returns(true);

                with.Module<BlobDelete>();
                with.Dependency(fakeProvider);
                with.Dependency(fakeTokensProvider);
            }));

            var result = browser.Delete("/blob/E9606D05-786A-4CFF-B708-E4B747E3A452/E9606D05-786A-4CFF-B708-E4B747E3A452/test.jpg", with => {
                with.HttpRequest();
            });

            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }
}
