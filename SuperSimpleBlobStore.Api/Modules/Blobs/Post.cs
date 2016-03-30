using System;
using System.Linq;
using System.Security.Claims;
using log4net;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using SuperSimpleBlobStore.Api.ViewModel.Blobs;
using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.Domain.Exceptions;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Modules.Blobs
{
    public class BlobPost : NancyModule
    {
        private static readonly ILog _logger = LogManager.GetLogger(ConfigurationProvider.ApplicationName);

        public BlobPost(IBlobs blobsProvider, ITokens tokensProvider)
            : base("/blob")
        {
            this.RequiresMSOwinAuthentication();

            Post["/"] = param =>
            {
                _logger.InfoFormat("POST: {0}", Context.Request.Path);

                var model = this.Bind<Blob>();
                var file = Request.Files.FirstOrDefault();

                if (model.ContainerIdentity == Guid.Empty || string.IsNullOrWhiteSpace(model.FileName) || file == null || file.Value.Length == 0)
                {
                    _logger.Error("POST: Could not create blob as preconditions are not met");
                    return HttpStatusCode.NotAcceptable;
                }

                try
                {
                    var tokenName = Context.GetMSOwinUser().Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    Guid token = Guid.Empty;

                    if (tokenName == null || !Guid.TryParse(tokenName.Value, out token) || !tokensProvider.CanAccessContainer(token, model.ContainerIdentity))
                    {
                        _logger.Info(string.Format("POST: Could not create blob as authentication preconditions are not met for token name '{0}' and token '{1}'", tokenName, token));
                        return HttpStatusCode.Forbidden;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("POST: Could not create blob as authorisation failed for token");
                    return HttpStatusCode.Forbidden;
                }

                // Read in the contents of the file
                byte[] fileContents = new byte[file.Value.Length];
                file.Value.Read(fileContents, 0, (int) file.Value.Length);

                Guid blobId;

                if (string.IsNullOrWhiteSpace(model.Etag))
                    model.Etag = Guid.NewGuid().ToString();

                if (string.IsNullOrWhiteSpace(model.ContentType))
                    model.ContentType = file.ContentType;

                try
                {
                    blobId = blobsProvider.CreateBlob(
                        fileContents,
                        model.FileName,
                        model.Etag,
                        model.ContentType,
                        model.ContainerIdentity,
                        Guid.NewGuid());
                }
                catch (CouldNotCreateBlobException ex)
                {
                    _logger.Error("POST: Could not create blob", ex);
                    return HttpStatusCode.InternalServerError;
                }

                _logger.InfoFormat("POST: '{0}' created blob with id of '{1}'", Context.Request.Path, blobId);

                return Response
                    .AsJson(new {BlobIdentity = blobId})
                    .WithStatusCode(HttpStatusCode.Created);
            };
        }
    }
}
