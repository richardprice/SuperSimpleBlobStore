using System;
using System.Collections.Generic;
using System.Security.Claims;
using FakeItEasy;
using Nancy;
using Nancy.Owin;
using Nancy.Testing;
using NUnit.Framework;
using SuperSimpleBlobStore.Api.Modules.Containers;
using SuperSimpleBlobStore.Api.Tests.Infrastructure;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Tests.Containers
{
    [TestFixture]
    public class ContainerPostTests : ApiTestBase
    {
        [Test]
        public void CanCreateContainer()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeContainersProvider = A.Fake<IContainers>();

                A.CallTo(() => fakeContainersProvider.CreateContainer(A<string>._, A<string>._, A<Guid>._))
                .WithAnyArguments()
                .Returns(Guid.NewGuid());

                with.Module<ContainerPost>();
                with.Dependency<IContainers>(fakeContainersProvider);

                with.RequestStartup((container, pipelines, context) =>
                {
                    context.Items[NancyMiddleware.RequestEnvironmentKey] = OwinUserInformation;
                });
            }));

            var result = browser.Post("/container", with => {
                with.HttpRequest();
                with.Header("content-type", "application/json");
                with.JsonBody( new { name = "test" } );
            });

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        }

        [Test]
        public void CanNotCreateContainerAsUnauthenticatedUser()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeContainersProvider = A.Fake<IContainers>();

                A.CallTo(() => fakeContainersProvider.CreateContainer(A<string>._, A<string>._, A<Guid>._))
                .WithAnyArguments()
                .Returns(Guid.NewGuid());

                with.Module<ContainerPost>();
                with.Dependency<IContainers>(fakeContainersProvider);
            }));

            var result = browser.Post("/container", with => {
                with.HttpRequest();
                with.Header("content-type", "application/json");
                with.JsonBody(new { name = "test" });
            });

            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
        }

    }
}
