using System;
using SuperSimpleBlobStore.DataAccess;
using SuperSimpleBlobStore.DataAccess.Model;
using SuperSimpleBlobStore.Domain.Exceptions;

namespace SuperSimpleBlobStore.Domain.Providers
{
    public class Tokens : ITokens
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IContainerRepository _containerRepository;

        public Tokens(ITokenRepository tokenRepository, IContainerRepository containerRepository)
        {
            _tokenRepository = tokenRepository;
            _containerRepository = containerRepository;
        }

        public Api.ViewModel.Tokens.AuthenticationToken CreateToken(Guid containerId, Guid creatorId, string description)
        {
            var container = _containerRepository.GetContainer(containerId);

            if(container == null)
                throw new CouldNotCreateTokenException();

            var token = new AuthenticationToken
            {
                PublicKey = Guid.NewGuid(),
                PrivateKey = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                CreatedBy = creatorId,
                ValidFrom = DateTime.Now,
                ContainerId = container.Id,
                Description = description
            };

            try
            {
                var result = _tokenRepository.CreateToken(token);

                if (result == null)
                    throw new CouldNotCreateTokenException();

                return new Api.ViewModel.Tokens.AuthenticationToken
                {
                    PublicKey = result.PublicKey,
                    PrivateKey = result.PrivateKey,
                    CreatedOn = result.CreatedOn,
                    CreatedBy = result.CreatedBy,
                    ContainerIdentity = container.ContainerIdentity,
                    ValidFrom = result.ValidFrom,
                    ValidTo = result.ValidTo,
                    Description = result.Description
                };
            }
            catch
            {
                throw new CouldNotCreateTokenException();
            }
        }

        public Api.ViewModel.Tokens.AuthenticationToken GetToken(Guid publicKey)
        {
            try
            {
                var token = _tokenRepository.GetToken(publicKey);
                Guid? containerIdentity = null;

                if (token == null)
                    throw new CouldNotGetTokenException();

                // If the token is restricted to a container, fetch the container identity
                if (token.ContainerId != null && token.ContainerId != 0)
                {
                    var container = _containerRepository.GetContainer((int)token.ContainerId);

                    if (container == null)
                        throw new CouldNotGetContainerException();

                    containerIdentity = container.ContainerIdentity;
                }

                return new Api.ViewModel.Tokens.AuthenticationToken
                {
                    PublicKey = token.PublicKey,
                    PrivateKey = token.PrivateKey,
                    CreatedOn = token.CreatedOn,
                    CreatedBy = token.CreatedBy,
                    ValidFrom = token.ValidFrom,
                    ValidTo = token.ValidTo,
                    ContainerIdentity = containerIdentity,
                    Description = token.Description
                };
            }
            catch
            {
                throw new CouldNotGetTokenException();
            }
        }

        public bool CanAccessContainer(Guid publicKey, Guid containerKey)
        {
            try
            {
                var token = _tokenRepository.GetToken(publicKey);

                if (token == null)
                    throw new CouldNotGetTokenException();

                // If the token is restricted to a container, fetch the container identity
                if (token.ContainerId != null && token.ContainerId != 0)
                {
                    var container = _containerRepository.GetContainer((int) token.ContainerId);

                    if (container == null)
                        throw new CouldNotGetContainerException();
                    
                    return container.ContainerIdentity == containerKey;
                }

                // Token is not restricted to specific container
                return true;
            }
            catch
            {
                throw new CouldNotGetTokenException();
            }
        }
    }
}
