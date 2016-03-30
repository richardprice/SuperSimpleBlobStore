using System;

namespace SuperSimpleBlobStore.Client
{
    public class BlobStoreResponse
    {
        public Guid BlobIdentity { get; set; }
        public Guid VersionIdentity { get; set; }
    }
}
