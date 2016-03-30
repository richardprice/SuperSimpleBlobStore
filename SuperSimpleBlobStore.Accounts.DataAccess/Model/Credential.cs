using SuperSimpleBlobStore.Accounts.DataAccess.Common;
using System;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public class Credential : IEntity
    {
        public Credential()
        {
        }

        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Hint { get; set; }
        public CryptoSchemes EncryptionScheme { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual Person Person { get; set; }
    }
}
