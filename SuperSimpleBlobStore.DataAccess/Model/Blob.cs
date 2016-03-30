using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.DataAccess.Model
{
    public class Blob
    {
        public Guid BlobId { get; set; }
        public Guid BlobCreatedBy { get; set; }
        public DateTime BlobCreatedOn { get; set; }
        public DateTime BlobValidFrom { get; set; }
        public DateTime? BlobValidTo { get; set; }
        public Guid ContainerIdentity { get; set; }
        public DateTime BlobLastModified { get; set; }
        public Guid BlobLastModifiedBy { get; set; }
        public Guid VersionId { get; set; }
        public DateTime VersionCreatedOn { get; set; }
        public DateTime VersionValidFrom { get; set; }
        public DateTime? VersionValidTo { get; set; }
        public DateTime VersionLastModified { get; set; }
        public Guid VersionLastModifiedBy { get; set; }
        public int BlobContentsId { get; set; }
        public string Etag { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string BlobVariantHash { get; set; }
    }
}
