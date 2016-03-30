using System;
using SuperSimpleBlobStore.Api.ViewModel.Containers;

namespace SuperSimpleBlobStore.Domain.Providers
{
    public interface IContainers
    {
        Guid CreateContainer(string containerName, string containerEtag, Guid creator);
        bool UpdateContainer(Container container);
        Container GetContainer(Guid id);
    }
}