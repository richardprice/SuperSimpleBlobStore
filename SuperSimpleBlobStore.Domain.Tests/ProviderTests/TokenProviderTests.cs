using System;
using FakeItEasy;
using NUnit.Framework;
using SuperSimpleBlobStore.DataAccess;
using SuperSimpleBlobStore.DataAccess.Model;
using SuperSimpleBlobStore.Domain.Exceptions;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Domain.Tests.ProviderTests
{
    [TestFixture]
    public class TokenProviderTests
    {
        [Test]
        public void CanCreateToken()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();
            var fakeTokenRepository = A.Fake<ITokenRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<Guid>._))
                .WithAnyArguments()
                .Returns(new Container
                {
                    ContainerIdentity = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    Etag = "nothing",
                    Id = 1,
                    LastModified = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    Name = "Fake container",
                    LastModifiedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")
                });

            A.CallTo(() => fakeTokenRepository.CreateToken(A<AuthenticationToken>._))
                .WithAnyArguments()
                .ReturnsLazily((AuthenticationToken token) => new AuthenticationToken()
                {
                    PublicKey = token.PublicKey,
                    PrivateKey = token.PrivateKey,
                    CreatedOn = token.CreatedOn,
                    CreatedBy = token.CreatedBy,
                    ContainerId = token.ContainerId,
                    ValidFrom = token.ValidFrom,
                    ValidTo = token.ValidTo
                });

            var tokenProvider = new Tokens(fakeTokenRepository, fakeContainerRepository);

            var result = tokenProvider.CreateToken(Guid.NewGuid(), Guid.NewGuid(), "Fake token");

            Assert.IsNotNull(result);
        }

        [Test]
        public void CanHandleFailureToCreateToken()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();
            var fakeTokenRepository = A.Fake<ITokenRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<Guid>._))
                .WithAnyArguments()
                .Returns(null);

            A.CallTo(() => fakeTokenRepository.CreateToken(A<AuthenticationToken>._))
                .WithAnyArguments()
                .ReturnsLazily((AuthenticationToken token) => new AuthenticationToken()
                {
                    PublicKey = token.PublicKey,
                    PrivateKey = token.PrivateKey,
                    CreatedOn = token.CreatedOn,
                    CreatedBy = token.CreatedBy,
                    ContainerId = token.ContainerId,
                    ValidFrom = token.ValidFrom,
                    ValidTo = token.ValidTo
                });

            var tokenProvider = new Tokens(fakeTokenRepository, fakeContainerRepository);

            Assert.Throws<CouldNotCreateTokenException>(
                delegate {
                    tokenProvider.CreateToken(Guid.NewGuid(), Guid.NewGuid(), "Fake token");
                });
        }

        [Test]
        public void CanGetToken()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();
            var fakeTokenRepository = A.Fake<ITokenRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<int>._))
                .WithAnyArguments()
                .Returns(new Container
                {
                    ContainerIdentity = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    Etag = "nothing",
                    Id = 1,
                    LastModified = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    Name = "Fake container",
                    LastModifiedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")
                });

            A.CallTo(() => fakeTokenRepository.GetToken(A<Guid>._))
                .WithAnyArguments()
                .Returns(new AuthenticationToken
                {
                    PublicKey = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    PrivateKey = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    ContainerId = 1,
                    ValidFrom = DateTime.Now
                });

            var tokenProvider = new Tokens(fakeTokenRepository, fakeContainerRepository);

            var result = tokenProvider.GetToken(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"));

            Assert.AreEqual(result.PrivateKey, Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"));
        }

        [Test]
        public void CanHandleExceptionWhenGettingToken()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();
            var fakeTokenRepository = A.Fake<ITokenRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<int>._))
                .WithAnyArguments()
                .Returns(null);

            A.CallTo(() => fakeTokenRepository.GetToken(A<Guid>._))
                .WithAnyArguments()
                .Returns(new AuthenticationToken
                {
                    PublicKey = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    PrivateKey = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    ContainerId = 1,
                    ValidFrom = DateTime.Now
                });

            var tokenProvider = new Tokens(fakeTokenRepository, fakeContainerRepository);

            Assert.Throws<CouldNotGetTokenException>(
                delegate {
                    tokenProvider.GetToken(Guid.NewGuid());
                });
        }

        [Test]
        public void TokenCanAccessContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();
            var fakeTokenRepository = A.Fake<ITokenRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<int>._))
                .WithAnyArguments()
                .Returns(new Container
                {
                    ContainerIdentity = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    Etag = "nothing",
                    Id = 1,
                    LastModified = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    Name = "Fake container",
                    LastModifiedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")
                });

            A.CallTo(() => fakeTokenRepository.GetToken(A<Guid>._))
                .WithAnyArguments()
                .Returns(new AuthenticationToken
                {
                    PublicKey = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    PrivateKey = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    ContainerId = 1,
                    ValidFrom = DateTime.Now
                });

            var tokenProvider = new Tokens(fakeTokenRepository, fakeContainerRepository);

            var result = tokenProvider.CanAccessContainer(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"));

            Assert.AreEqual(result, true);
        }

        [Test]
        public void TokenCannotAccessContainer()
        {
            var fakeContainerRepository = A.Fake<IContainerRepository>();
            var fakeTokenRepository = A.Fake<ITokenRepository>();

            A.CallTo(() => fakeContainerRepository.GetContainer(A<int>._))
                .WithAnyArguments()
                .Returns(new Container
                {
                    ContainerIdentity = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    Etag = "nothing",
                    Id = 1,
                    LastModified = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    Name = "Fake container",
                    LastModifiedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}")
                });

            A.CallTo(() => fakeTokenRepository.GetToken(A<Guid>._))
                .WithAnyArguments()
                .Returns(new AuthenticationToken
                {
                    PublicKey = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    PrivateKey = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    CreatedOn = DateTime.Now,
                    CreatedBy = Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"),
                    ContainerId = 1,
                    ValidFrom = DateTime.Now
                });

            var tokenProvider = new Tokens(fakeTokenRepository, fakeContainerRepository);

            var result = tokenProvider.CanAccessContainer(Guid.Parse("{E9606D05-786A-4CFF-B708-E4B747E3A452}"), Guid.Parse("{70C7D0EF-6685-4224-A6E1-A095D9C308CF}"));

            Assert.AreEqual(result, false);
        }
    }
}
