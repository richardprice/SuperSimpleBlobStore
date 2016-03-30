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
    public class BlobPut : NancyModule
    {
        private static readonly ILog _logger = LogManager.GetLogger(ConfigurationProvider.ApplicationName);

        public BlobPut(IBlobs blobsProvider, ITokens tokensProvider)
            : base("/blob")
        {
            this.RequiresMSOwinAuthentication();

            Put["/"] = param =>
            {
                _logger.InfoFormat("PUT: {0}", Context.Request.Path);

                var model = this.Bind<Blob>();
                var file = Request.Files.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(model.FileName) || file == null || file.Value.Length == 0)
                {
                    _logger.Error("PUT: Could not update blob as preconditions are not met");
                    return Response.AsJson("").WithStatusCode(HttpStatusCode.NotAcceptable);
                }
                
                Blob blob;

                try
                {
                    blob = blobsProvider.GetBlob(model.BlobId);
                }
                catch (CouldNotGetBlobException ex)
                {
                    _logger.Error("PUT: Could not update blob as GetBlob threw exception", ex);
                    return HttpStatusCode.NotFound;
                }
                catch (BlobNotFoundException ex)
                {
                    _logger.Info(string.Format("PUT: Could not update blob as blob not found"));
                    return HttpStatusCode.NotFound;
                }

                if (blob == null)
                {
                    _logger.Error("PUT: Could not update blob as GetBlob threw exception");
                    return HttpStatusCode.NotFound;
                }

                try
                {
                    if (blob.ContainerIdentity != model.ContainerIdentity)
                    {
                        return HttpStatusCode.Forbidden;
                    }

                    var tokenName = Context.GetMSOwinUser().Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    Guid token = Guid.Empty;

                    if (tokenName == null || !Guid.TryParse(tokenName.Value, out token) || !tokensProvider.CanAccessContainer(token, model.ContainerIdentity))
                    {
                        _logger.Info(string.Format("PUT: Could not update blob as authentication preconditions are not met for token name '{0}' and token '{1}'", tokenName, token));
                        return HttpStatusCode.Forbidden;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("PUT: Could not update blob as authorisation failed for token", ex);
                    return HttpStatusCode.Forbidden;
                }

                // Read in the contents of the file
                byte[] fileContents = new byte[file.Value.Length];
                file.Value.Read(fileContents, 0, (int)file.Value.Length);

                Guid versionId;

                if (string.IsNullOrWhiteSpace(model.Etag))
                    model.Etag = Guid.NewGuid().ToString();

                if (string.IsNullOrWhiteSpace(model.ContentType))
                    model.ContentType = file.ContentType;

                try
                {
                    versionId = blobsProvider.CreateBlobVersion(
                        model.BlobId,
                        fileContents,
                        model.FileName,
                        model.Etag,
                        model.ContentType,
                        Guid.NewGuid());
                }
                catch (CouldNotCreateBlobVersionException ex)
                {
                    _logger.Error("PUT: Could not update blob version", ex);
                    return HttpStatusCode.InternalServerError;
                }

                _logger.InfoFormat("PUT: '{0}' updated blob with id of '{1}' and version of '{2}'", Context.Request.Path, model.BlobId, versionId);

                return Response
                    .AsJson(new { BlobIdentity = model.BlobId, VersionIdentity = versionId })
                    .WithStatusCode(HttpStatusCode.Created);
            };
        }
    }
}
