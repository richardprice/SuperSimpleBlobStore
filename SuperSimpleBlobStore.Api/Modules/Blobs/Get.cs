using System;
using System.Globalization;
using log4net;
using Nancy;
using SuperSimpleBlobStore.Api.Infrastructure;
using SuperSimpleBlobStore.Api.ViewModel.Blobs;
using SuperSimpleBlobStore.Common;
using SuperSimpleBlobStore.Domain.Exceptions;
using SuperSimpleBlobStore.Domain.Providers;

namespace SuperSimpleBlobStore.Api.Modules.Blobs
{
    public class BlobGet : NancyModule
    {
        private static readonly ILog _logger = LogManager.GetLogger(ConfigurationProvider.ApplicationName);

        public BlobGet(IBlobs blobsProvider)
            : base("/blob")
        {
            Get["/{id:guid}/{filename*}"] = param =>
            {
                _logger.InfoFormat("GET: {0}", Context.Request.Path);

                if (param.id == null)
                {
                    _logger.Error(string.Format("GET: Could not get '{0}' as QueryString id value is null", Context.Request.Path));
                    return Response.AsJson("").WithStatusCode(HttpStatusCode.NotAcceptable);
                }

                Blob blob;
                var filename = (string) param.filename;

                if (Context.Request.Query["date"] == null)
                {
                    try
                    {
                        blob = blobsProvider.GetBlob((Guid) param.id);
                    }
                    catch (CouldNotGetBlobException ex)
                    {
                        _logger.Error(
                            string.Format("GET: Could not get '{0}' as GetBlob threw exception", Context.Request.Path),
                            ex);
                        return
                            Response.AsJson(new {errorType = ex.GetType().FullName, errorMessage = ex.Message})
                                .WithStatusCode(HttpStatusCode.InternalServerError);
                    }
                    catch (BlobNotFoundException ex)
                    {
                        _logger.Info(string.Format("GET: Could not get '{0}' as blob not found", Context.Request.Path));
                        return HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    DateTime specificBlobDate;

                    if (!DateTime.TryParse(Context.Request.Query["date"], out specificBlobDate))
                    {
                        return HttpStatusCode.NotAcceptable;
                    }

                    try
                    {
                        blob = blobsProvider.GetBlobForDate((Guid)param.id, specificBlobDate);
                    }
                    catch (CouldNotGetBlobException ex)
                    {
                        _logger.Error(string.Format("GET: Could not get '{0}' as GetBlobForDate response threw exception", Context.Request.Path), ex);
                        return
                            Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message })
                                .WithStatusCode(HttpStatusCode.InternalServerError);
                    }
                    catch (BlobNotFoundException ex)
                    {
                        _logger.Info(string.Format("GET: Could not get '{0}' as blob not found", Context.Request.Path));
                        return HttpStatusCode.NotFound;
                    }
                }
                

                if (blob == null)
                {
                    _logger.Error(string.Format("GET: Could not get '{0}' as blob response is null", Context.Request.Path));
                    return Response.AsJson("").WithStatusCode(HttpStatusCode.NotFound);
                }

                byte[] contents;

                try
                {
                    contents = blobsProvider.GetContents((Guid) param.id);
                }
                catch (CouldNotGetBlobException ex)
                {
                    _logger.Error(string.Format("GET: Could not get '{0}' as GetContents threw exception", Context.Request.Path), ex);
                    return Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message }).WithStatusCode(HttpStatusCode.InternalServerError);
                }
                catch (BlobNotFoundException ex)
                {
                    _logger.Info(string.Format("GET: Could not get '{0}' as blob contents not found", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                _logger.InfoFormat("GET: Successfully fetched '{0}'", Context.Request.Path);

                return Response.FromByteArray(contents, blob.ContentType)
                    .WithHeader("Content-Length", contents.Length.ToString())
                    .WithHeader("Content-Type", blob.ContentType)
                    .WithHeader("Etag", blob.Etag)
                    .WithHeader("Last-Modified", blob.VersionLastModified.ToUniversalTime().ToString("r"))
                    .WithHeader("Date", DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture))
                    .WithHeader("Content-Disposition", string.Format("{0}; filename={1};", (Context.Request.Query["dl"] != null ? "attachment" : "inline"), filename))
                    .WithStatusCode(HttpStatusCode.OK);
            };

            Get["/{id:guid}/{versionId:guid}/{filename*}"] = param =>
            {
                _logger.InfoFormat("GET: {0}", Context.Request.Path);

                if (param.id == null || param.versionId == null)
                {
                    _logger.Error(string.Format("GET: Could not get '{0}' as QueryString id value is null", Context.Request.Path));
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
                    _logger.Error(string.Format("GET: Could not get '{0}' as GetBlobVersion response threw exception", Context.Request.Path), ex);
                    return Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message }).WithStatusCode(HttpStatusCode.InternalServerError);
                }
                catch (BlobNotFoundException ex)
                {
                    _logger.Info(string.Format("GET: Could not get '{0}' as blob not found", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                if (blob == null)
                {
                    _logger.Error(string.Format("GET: Could not get '{0}' as blob response is null", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                byte[] contents;

                try
                {
                    contents = blobsProvider.GetContentsForVersion((Guid)param.id, (Guid)param.versionId);
                }
                catch (CouldNotGetBlobException ex)
                {
                    _logger.Error(string.Format("GET: Could not get '{0}' as GetContents threw exception", Context.Request.Path), ex);
                    return Response.AsJson(new { errorType = ex.GetType().FullName, errorMessage = ex.Message }).WithStatusCode(HttpStatusCode.InternalServerError);
                }
                catch (BlobNotFoundException ex)
                {
                    _logger.Info(string.Format("GET: Could not get '{0}' as blob contents not found", Context.Request.Path));
                    return HttpStatusCode.NotFound;
                }

                _logger.InfoFormat("GET: Successfully fetched '{0}'", Context.Request.Path);

                return Response.FromByteArray(contents, blob.ContentType)
                    .WithHeader("Content-Length", blob.ContentLength.ToString())
                    .WithHeader("Content-Type", blob.ContentType)
                    .WithHeader("Etag", blob.Etag)
                    .WithHeader("Last-Modified", blob.VersionLastModified.ToUniversalTime().ToString("r"))
                    .WithHeader("Date", DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture))
                    .WithHeader("Content-Disposition", string.Format("{0}; filename={1};", (Context.Request.Query["dl"] != null ? "attachment" : "inline"), filename))
                    .WithStatusCode(HttpStatusCode.OK);
            };
        }
    }
}
