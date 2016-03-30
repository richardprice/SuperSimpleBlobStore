using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleBlobStore.Api.ViewModel.Tokens
{
    public class AuthenticationToken
    {
        public Guid PublicKey { get; set; }
        public Guid PrivateKey { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public Guid? ContainerIdentity { get; set; }
        public string Description { get; set; }
    }
}
