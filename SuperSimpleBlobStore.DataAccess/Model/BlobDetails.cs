using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.DataAccess.Model
{
    public class BlobDetails
    {
        public int Id { get; set; }
        public Guid BlobId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int ContainerId { get; set; }
        public DateTime LastModified { get; set; }
        public Guid LastModifiedBy { get; set; }
    }
}
