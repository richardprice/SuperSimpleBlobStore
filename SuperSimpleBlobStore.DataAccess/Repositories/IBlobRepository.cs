using System;
using System.Collections.Generic;
using SuperSimpleBlobStore.DataAccess.Model;

namespace SuperSimpleBlobStore.DataAccess
{
    public interface IBlobRepository
    {
        Blob GetLatestValidBlob(Guid id);
        Blob GetSpecificBlobVersion(Guid id, Guid versionId);
        byte[] GetLatestValidBlobContents(Guid id);
        Blob GetBlobForDate(Guid id, DateTime date);
        BlobDetails GetBlobDetails(int id);
        BlobDetails GetBlobDetails(Guid id);
        IEnumerable<BlobDetails> GetBlobsForOwner(Guid id);
        IEnumerable<BlobDetails> GetBlobsForContainer(Guid id);
        BlobDetails CreateBlobDetails(BlobDetails blob);
        bool UpdateBlobDetails(BlobDetails blob);
        bool DeleteBlobDetails(BlobDetails blob);
        BlobVersion GetBlobVersion(int id);
        BlobVersion GetBlobVersion(Guid id);
        BlobVersion CreateBlobVersion(BlobVersion blob);
        bool UpdateBlobVersion(BlobVersion blob);
        bool DeleteBlobVersion(BlobVersion blob);
        BlobContents GetBlobContents(int id);
        BlobContents CreateBlobContents(BlobContents blob);
    }
}