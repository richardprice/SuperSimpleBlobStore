using System;
using System.Linq;
using System.Security.Claims;
using log4net;
using Nancy;
using Nancy.Security;
using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.Domain.Exceptions;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Modules.Blobs
{
    public class BlobDelete : NancyModule
    {
        private static readonly ILog _logger = LogManager.GetLogger(ConfigurationProvider.ApplicationName);

        public BlobDelete(IBlobs blobsProvider, ITokens tokensProvider)
            : base("/blob")
        {
            this.RequiresMSOwinAuthentication();

            Delete["/{id:guid}/{filename*}"] = param =>
            {
                _logger.InfoFormat("DELETE: {0}", Context.Request.Path);

                if (param.id == null)
                {
                    _logger.Error(string.Format("DELETE: Could not delete '{0}' as QueryString id value is null", Context.Request.Path));
                    return HttpStatusCode.NotAcceptable;
                }

                try
                {
                    var blob = blobsProvider.GetBlob((Guid) param.id);
                    var tokenName = Context.GetMSOwinUser().Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    Guid token = Guid.Empty;

                    if (tokenName == null || !Guid.TryParse(tokenName.Value, out token) || !tokensProvider.CanAccessContainer(token, blob.ContainerIdentity))
                    {
                        _logger.Info(string.Format("DELETE: Could not delete blob as authentication preconditions are not met for token name '{0}' and token '{1}'", tokenName, token));
                        return HttpStatusCode.Forbidden;
                    }
                }
                catch (CouldNotGetBlobException ex)
                {
                    _logger.Error(string.Format("DELETE: Could not delete '{0}' as GetBlob threw exception", Context.Request.Path), ex);
                    return HttpStatusCode.NotFound;
                }

                bool deletionResult;

                try
                {
                    deletionResult = blobsProvider.DeleteBlob((Guid) param.id);
                }
                catch (CouldNotGetBlobException ex)
                {
                    _logger.Error(string.Format("DELETE: Could not delete '{0}' as GetBlob threw exception", Context.Request.Path), ex);
                    return HttpStatusCode.NotFound;
                }
                catch (BlobNotFoundException ex)
                {
                    _logger.Info(string.Format("DELETE: Could not delete '{0}' as blob not found", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                if (deletionResult)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.InternalServerError;
                }
            };

            Delete["/{id:guid}/{versionId:guid}/{filename*}"] = param =>
            {
                _logger.InfoFormat("DELETE: {0}", Context.Request.Path);

                if (param.id == null || param.versionId == null)
                {
                    _logger.Error(string.Format("DELETE: Could not delete '{0}' as QueryString id or versionId value is null", Context.Request.Path));
                    return HttpStatusCode.NotAcceptable;
                }

                try
                {
                    var blob = blobsProvider.GetBlob((Guid)param.id);
                    var tokenName = Context.GetMSOwinUser().Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    Guid token = Guid.Empty;

                    if (tokenName == null || !Guid.TryParse(tokenName.Value, out token) || !tokensProvider.CanAccessContainer(token, blob.ContainerIdentity))
                    {
                        _logger.Info(string.Format("DELETE: Could not delete blob as authentication preconditions are not met for token name '{0}' and token '{1}'", tokenName, token));

                        return HttpStatusCode.Forbidden;
                    }
                }
                catch (CouldNotGetBlobException ex)
                {
                    _logger.Error(string.Format("DELETE: Could not delete '{0}' as GetBlob threw exception", Context.Request.Path), ex);
                    return HttpStatusCode.NotFound;
                }
                catch (BlobNotFoundException ex)
                {
                    _logger.Info(string.Format("DELETE: Could not delete '{0}' as blob not found", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                bool deletionResult;

                try
                {
                    deletionResult = blobsProvider.DeleteBlobVersion((Guid)param.id, (Guid)param.versionId);
                }
                catch (CouldNotGetBlobException ex)
                {
                    _logger.Error(string.Format("DELETE: Could not delete '{0}' as GetBlob threw exception", Context.Request.Path), ex);
                    return HttpStatusCode.NotFound;
                }
                catch (BlobNotFoundException ex)
                {
                    _logger.Info(string.Format("DELETE: Could not delete '{0}' as blob not found", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                if (deletionResult)
                {
                    return HttpStatusCode.OK;
                }
                else
                {
                    return HttpStatusCode.InternalServerError;
                }
            };
        }
    }
}
