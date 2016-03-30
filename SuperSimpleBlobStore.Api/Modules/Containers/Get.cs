using System;
using Nancy;
using SuperSimpleBlobStore.Api.ViewModel.Containers;
using SuperSimpleBlobStore.Domain.Exceptions;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Modules.Containers
{
    public class ContainerGet : NancyModule
    {
        public ContainerGet(IContainers containersProvider)
            : base("/container")
        {
            Get["/{id:guid}"] = param =>
            {
                
                if (param.id == null)
                {
                    return Response.AsJson("").WithStatusCode(HttpStatusCode.NotAcceptable);
                }

                Container container;

                try
                {
                    container = containersProvider.GetContainer((Guid)param.id);
                }
                catch (CouldNotGetContainerException ex)
                {
                    return Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message }).WithStatusCode(HttpStatusCode.InternalServerError);
                }

                if (container == null)
                {
                    return Response.AsJson("").WithStatusCode(HttpStatusCode.NotFound);
                }

                return Response.AsJson(container).WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}
