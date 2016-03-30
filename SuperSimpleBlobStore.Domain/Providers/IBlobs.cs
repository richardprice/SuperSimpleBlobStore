using System;
using SuperSimpleBlobStore.Api.ViewModel.Blobs;

namespace SuperSimpleBlobStore.Domain.Providers
{
    public interface IBlobs
    {
        Blob GetBlob(Guid id);
        Blob GetBlobVersion(Guid id, Guid versionId);
        Blob GetBlobForDate(Guid id, DateTime date);
        byte[] GetContents(Guid id);
        byte[] GetContentsForVersion(Guid id, Guid versionId);
        Guid CreateBlob(byte[] contents, string fileName, string etag, string mimeType, Guid containerIdentity, Guid creatorId);
        Guid CreateBlobVersion(Guid blobId, byte[] contents, string fileName, string etag, string mimeType, Guid creatorId);
        bool DeleteBlob(Guid id);
        bool DeleteBlobVersion(Guid id, Guid versionId);
    }
}