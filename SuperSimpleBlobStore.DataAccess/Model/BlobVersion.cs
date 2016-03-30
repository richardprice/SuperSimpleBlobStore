using System;

namespace SuperSimpleBlobStore.DataAccess.Model
{
    public class BlobVersion
    {
        public int Id { get; set; }
        public Guid VersionId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime LastModified { get; set; }
        public Guid LastModifiedBy { get; set; }
        public int BlobContentsId { get; set; }
        public string Etag { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public int BlobId { get; set; }
        public string BlobVariantHash { get; set; }
    }
}
