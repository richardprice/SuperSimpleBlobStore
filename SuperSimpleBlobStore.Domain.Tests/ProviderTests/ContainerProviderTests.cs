using System;
using FakeItEasy;
using NUnit.Framework;
using SuperSimpleBlobStore.Api.ViewModel;
using SuperSimpleBlobStore.DataAccess;
using SuperSimpleBlobStore.DataAccess.Model;
using SuperSimpleBlobStore.Domain.Exceptions;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Domain.Tests.ProviderTests
{
    [TestFixture]
    public class ContainerProviderTests
    {
        [Test]
        public void CanCreateContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeContainerRepository.CreateContainer(A<Container>._))
                .WithAnyArguments()
                .ReturnsLazily((Container container) => new Container() {
                        ContainerIdentity = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                        CreatedBy = container.CreatedBy,
                        CreatedOn = DateTime.Now,
                        Etag = container.Etag,
                        Id = 1,
                        LastModified = DateTime.Now,
                        ValidFrom = DateTime.Now,
                        Name = container.Name,
                        LastModifiedBy = container.LastModifiedBy
                        });

            var containerProvider = new Containers(fakeContainerRepository);

            var result = containerProvider.CreateContainer("Test Container", "Test Etag", Guid.NewGuid());

            Assert.AreEqual(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), result);
        }

        [Test]
        public void CanHandleFailureToCreateContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeContainerRepository.CreateContainer(A<Container>._))
                .WithAnyArguments()
                .Returns(null);

            var containerProvider = new Containers(fakeContainerRepository);

            Assert.Throws<CouldNotCreateContainerException>(
                delegate {
                    containerProvider.CreateContainer("Test Container", "Test Etag", Guid.NewGuid());
                });
        }

        [Test]
        public void CanUpdateContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<Guid>._))
                .WithAnyArguments()
                .Returns(new Container());

            A.CallTo(() => fakeContainerRepository.UpdateContainer(A<Container>._))
                .WithAnyArguments()
                .Returns(true);

            var containerProvider = new Containers(fakeContainerRepository);

            var result = containerProvider.UpdateContainer(new Api.ViewModel.Containers.Container());

            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandleFailureToUpdateContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<Guid>._))
                .WithAnyArguments()
                .Returns(new Container());

            A.CallTo(() => fakeContainerRepository.UpdateContainer(A<Container>._))
                .WithAnyArguments()
                .Returns(false);

            var containerProvider = new Containers(fakeContainerRepository);

            var result = containerProvider.UpdateContainer(new Api.ViewModel.Containers.Container());

            Assert.IsFalse(result);
        }

        [Test]
        public void CanHandleExceptionWhenUpdatingContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<Guid>._))
                .WithAnyArguments()
                .Returns(null);

            A.CallTo(() => fakeContainerRepository.UpdateContainer(A<Container>._))
                .WithAnyArguments()
                .Returns(false);

            var containerProvider = new Containers(fakeContainerRepository);

            Assert.Throws<CouldNotUpdateContainerException>(
                delegate {
                             containerProvider.UpdateContainer(new Api.ViewModel.Containers.Container());
                });
        }

        [Test]
        public void CanGetContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<Guid>._))
                .WithAnyArguments()
                .Returns(new Container { Id = 1, Name = "FakeItEasy Container" });

            var containerProvider = new Containers(fakeContainerRepository);

            var result = containerProvider.GetContainer(Guid.NewGuid());

            Assert.AreEqual(result.Name, "FakeItEasy Container");
        }

        [Test]
        public void CanHandleExceptionWhenGettingContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<Guid>._))
                .WithAnyArguments()
                .Returns(null);

            var containerProvider = new Containers(fakeContainerRepository);

            Assert.Throws<CouldNotGetContainerException>(
                delegate {
                    containerProvider.GetContainer(Guid.NewGuid());
                });
        }
    }
}
