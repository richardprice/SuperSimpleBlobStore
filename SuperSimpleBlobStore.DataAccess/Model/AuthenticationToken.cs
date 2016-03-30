using System;

namespace SuperSimpleBlobStore.DataAccess.Model
{
    public class AuthenticationToken
    {
        public int Id { get; set; }
        public Guid PublicKey { get; set; }
        public Guid PrivateKey { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int? ContainerId { get; set; }
        public string Description { get; set; }
    }
}
