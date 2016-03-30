using SuperSimpleBlobStore.Accounts.DataAccess.Common;
using System.Collections.Generic;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public class Claim : IAttribute
    {
        public Claim()
        {
            People = new List<Person>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public virtual ICollection<Person> People { get; set; }

        public SuperSimpleBlobStore.Common.Attribute ToAttribute()
        {
            return new SuperSimpleBlobStore.Common.Attribute(Id = Id, Name = Name, SortOrder = SortOrder);
        }
    }
}
