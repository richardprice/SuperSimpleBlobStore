using System;
using System.Text;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using SuperSimpleBlobStore.Api.ViewModel.Containers;
using SuperSimpleBlobStore.Domain.Exceptions;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Modules.Containers
{
    public class ContainerPost : NancyModule
    {
        public ContainerPost(IContainers containersProvider)
            : base("/container")
        {
            this.RequiresMSOwinAuthentication();

            Post["/"] = _ =>
            {
                var model = this.Bind<Container>();

                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return Response.AsJson("").WithStatusCode(HttpStatusCode.NotAcceptable);
                }

                Guid? result;

                try
                {
                    result = containersProvider.CreateContainer(
                        model.Name,
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid());
                }
                catch (CouldNotCreateContainerException ex)
                {
                    return Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message }).WithStatusCode(HttpStatusCode.InternalServerError);
                }

                return Response.AsJson(new { ContainerIdentity = result }).WithStatusCode(HttpStatusCode.Created);
            };
        }
    }
}