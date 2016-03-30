using System;
using System.Globalization;
using log4net;
using Nancy;
using SuperSimpleBlobStore.Api.ViewModel.Blobs;
using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.Domain.Exceptions;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Modules.Blobs
{
    public class BlobHead : NancyModule
    {
        private static readonly ILog _logger = LogManager.GetLogger(ConfigurationProvider.ApplicationName);

        public BlobHead(IBlobs blobsProvider)
            : base("/blob")
        {
            Head["/{id:guid}/{filename*}"] = param =>
            {

                _logger.InfoFormat("HEAD: {0}", Context.Request.Path);

                if (param.id == null)
                {
                    _logger.Error(string.Format("HEAD: Could not get '{0}' as QueryString id value is null", Context.Request.Path));
                    return Response.AsJson("").WithStatusCode(HttpStatusCode.NotAcceptable);
                }

                Blob blob;
                var filename = (string)param.filename;

                if (Context.Request.Query["date"] == null)
                {
                    try
                    {
                        blob = blobsProvider.GetBlob((Guid)param.id);
                    }
                    catch (CouldNotGetBlobException ex)
                    {
                        _logger.Error(string.Format("HEAD: Could not get '{0}' as GetBlob threw exception", Context.Request.Path), ex);
                        return
                            Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message })
                                .WithStatusCode(HttpStatusCode.InternalServerError);
                    }
                    catch (BlobNotFoundException ex)
                    {
                        _logger.Info(string.Format("HEAD: Could not get '{0}' as blob not found", Context.Request.Path));
                        return HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    DateTime specificBlobDate;

                    if (!DateTime.TryParse(Context.Request.Query["date"], out specificBlobDate))
                    {
                        return Response.AsJson("").WithStatusCode(HttpStatusCode.NotAcceptable);
                    }

                    try
                    {
                        blob = blobsProvider.GetBlobForDate((Guid)param.id, specificBlobDate);
                    }
                    catch (CouldNotGetBlobException ex)
                    {
                        _logger.Error(string.Format("HEAD: Could not get '{0}' as GetBlobForDate response threw exception", Context.Request.Path), ex);
                        return
                            Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message })
                                .WithStatusCode(HttpStatusCode.InternalServerError);
                    }
                    catch (BlobNotFoundException ex)
                    {
                        _logger.Info(string.Format("HEAD: Could not get '{0}' as blob not found", Context.Request.Path));
                        return HttpStatusCode.NotFound;
                    }
                }

                if (blob == null)
                {
                    _logger.Error(string.Format("HEAD: Could not get '{0}' as blob response is null", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                _logger.InfoFormat("HEAD: Successfully fetched '{0}'", Context.Request.Path);
                

                return new HeadResponse(Response.AsText("")
                    .WithHeader("Content-Length", blob.ContentLength.ToString())
                    .WithHeader("Content-Type", blob.ContentType)
                    .WithHeader("Etag", blob.Etag)
                    .WithHeader("Last-Modified", blob.VersionLastModified.ToUniversalTime().ToString("r"))
                    .WithHeader("Date", DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture))
                    .WithStatusCode(HttpStatusCode.OK));
            };

            Head["/{id:guid}/{versionId:guid}/{filename*}"] = param =>
            {

                _logger.InfoFormat("HEAD: {0}", Context.Request.Path);

                if (param.id == null || param.versionId == null)
                {
                    _logger.Error(string.Format("HEAD: Could not get '{0}' as QueryString id value is null", Context.Request.Path));
                    return Response.AsJson("").WithStatusCode(HttpStatusCode.NotAcceptable);
                }

                Blob blob;
                string filename = (string)param.filename;

                try
                {
                    blob = blobsProvider.GetBlobVersion((Guid)param.id, (Guid)param.versionId);
                }
                catch (CouldNotGetBlobException ex)
                {
                    _logger.Error(string.Format("HEAD: Could not get '{0}' as GetBlobVersion response threw exception", Context.Request.Path), ex);
                    return Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message }).WithStatusCode(HttpStatusCode.InternalServerError);
                }
                catch (BlobNotFoundException ex)
                {
                    _logger.Info(string.Format("HEAD: Could not get '{0}' as blob not found", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                if (blob == null)
                {
                    _logger.Error(string.Format("HEAD: Could not get '{0}' as blob response is null", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                _logger.InfoFormat("HEAD: Successfully fetched '{0}'", Context.Request.Path);

                return new HeadResponse(Response.AsText("")
                    .WithHeader("Content-Length", blob.ContentLength.ToString())
                    .WithHeader("Content-Type", blob.ContentType)
                    .WithHeader("Etag", blob.Etag)
                    .WithHeader("Last-Modified", blob.VersionLastModified.ToUniversalTime().ToString("r"))
                    .WithHeader("Date", DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture))
                    .WithStatusCode(HttpStatusCode.OK));
            };
        }
    }
}
