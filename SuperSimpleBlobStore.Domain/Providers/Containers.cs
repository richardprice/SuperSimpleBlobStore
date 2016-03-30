using System;
using SuperSimpleBlobStore.Api.ViewModel.Containers;
using SuperSimpleBlobStore.DataAccess;
using SuperSimpleBlobStore.Domain.Exceptions;

namespace SuperSimpleBlobStore.Domain.Providers
{
    public class Containers : IContainers
    {
        private readonly IContainerRepository _containerRepository;

        public Containers(IContainerRepository containerRepository)
        {
            _containerRepository = containerRepository;
        }

        public Guid CreateContainer(string containerName, string containerEtag, Guid creator)
        {

            var container = new DataAccess.Model.Container
            {
                ContainerIdentity = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                CreatedBy = creator,
                Etag = containerEtag,
                Name = containerName,
                ValidFrom = DateTime.Now,
                LastModified = DateTime.Now,
                LastModifiedBy = creator
            };

            try
            {
                var result = _containerRepository.CreateContainer(container);

                if(result == null)
                    throw new CouldNotCreateContainerException();

                return result.ContainerIdentity;
            }
            catch
            {
                throw new CouldNotCreateContainerException();
            }
        }

        public bool UpdateContainer(Container container)
        {
            var containerToUpdate = _containerRepository.GetContainer(container.ContainerIdentity);

            if(containerToUpdate == null)
                throw new CouldNotUpdateContainerException();

            try
            {
                return _containerRepository.UpdateContainer(containerToUpdate);
            }
            catch
            {
                throw new CouldNotUpdateContainerException();
            }
        }

        public Container GetContainer(Guid id)
        {
            try
            {
                var container = _containerRepository.GetContainer(id);

                if (container == null)
                    throw new CouldNotGetContainerException();

                return new Container
                {
                    ContainerIdentity = container.ContainerIdentity,
                    CreatedOn = container.CreatedOn,
                    CreatedBy = container.CreatedBy,
                    Etag = container.Etag,
                    LastModified = container.LastModified,
                    LastModifiedBy = container.LastModifiedBy,
                    Name = container.Name,
                    ValidFrom = container.ValidFrom,
                    ValidTo = container.ValidTo
                };
            }
            catch
            {
                throw new CouldNotGetContainerException();
            }
        }
    }
}
