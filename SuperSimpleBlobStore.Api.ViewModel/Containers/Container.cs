using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Api.ViewModel.Containers
{
    public class Container
    {
        public Guid ContainerIdentity { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Etag { get; set; }
        public DateTime LastModified { get; set; }
        public Guid LastModifiedBy { get; set; }
    }
}
