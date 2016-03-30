using System;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using SuperSimpleBlobStore.Api.Modules.Containers;
using SuperSimpleBlobStore.Api.ViewModel.Containers;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Tests.Containers
{
    [TestFixture]
    public class ContainerGetTests
    {
        [Test]
        public void CanGetContainer()
        {
            var browser = new Browser(new ConfigurableBootstrapper(with =>
            {
                var fakeContainersProvider = A.Fake<IContainers>();

                A.CallTo(() => fakeContainersProvider.GetContainer(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")))
                .Returns(new Container
                {
                    ContainerIdentity = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedBy = Guid.Parse("{EF04F6D8-7492-46E9-8AE4-93655512BA74}"),
                    CreatedOn = DateTime.Now,
                    Etag = "{EF04F6D8-7492-46E9-8AE4-93655512BA74}",
                    LastModified = DateTime.Now,
                    LastModifiedBy = Guid.Parse("{EF04F6D8-7492-46E9-8AE4-93655512BA74}"),
                    Name = "Test Container",
                    ValidFrom = DateTime.Now
                });

                with.Module<ContainerGet>();
                with.Dependency(fakeContainersProvider);
            }));

            var result = browser.Get("/container/E9606D05-786A-4CFF-B708-E4B747E3A452", with => {
                with.HttpRequest();
            });

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
