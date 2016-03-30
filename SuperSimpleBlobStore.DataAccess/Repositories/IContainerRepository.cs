using System;
using System.Collections.Generic;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess
{
    public interface IContainerRepository
    {
        Container GetValidContainer(int id);
        Container GetValidContainer(Guid id);
        Container GetContainer(int id);
        Container GetContainer(Guid id);
        IEnumerable<Container> GetContainersForOwner(Guid id);
        Container CreateContainer(Container container);
        bool UpdateContainer(Container container);
        bool DeleteContainer(Container container);
    }
}