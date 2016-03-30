using SuperSimpleBlobStore.Accounts.DataAccess.Common;
using System;
using System.Collections.Generic;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public class Person : IEntity
    {
        public Person()
        {
            Credentials = new List<Credential>();
            Claims = new List<Claim>();
            VerificationTokens = new List<VerificationToken>();
        }

        public int Id { get; set; }
        public Guid AuthenticationId { get; set; }
        public string Name { get; set; }
        public DateTime? LastAuthenticated { get; set; }
        public DateTime? LastFailedAuthentication { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual ICollection<Claim> Claims { get; set; }
        public virtual ICollection<VerificationToken> VerificationTokens { get; set; }
    }
}
