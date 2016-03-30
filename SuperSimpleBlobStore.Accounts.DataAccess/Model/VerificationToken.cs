using SuperSimpleBlobStore.Accounts.DataAccess.Common;
using System;
using SuperSimpleBlobStore.Common;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public class VerificationToken : IEntity
    {
        public VerificationToken()
        {
        }

        public int Id { get; set; }
        public TokenUses Reason { get; set; }
        public Guid Token { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual Person Person { get; set; }
    }
}
