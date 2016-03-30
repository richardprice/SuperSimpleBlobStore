using System;
using System.Collections.Generic;

namespace SuperSimpleBlobStore.Api.ViewModel
{
    public class Person
    {
        public Person()
        {
            Claims = new List<string>();    
        }

        public int Id { get; set; }
        public Guid AuthenticationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public DateTime? LastAuthenticated { get; set; }
        public DateTime? LastFailedAuthentication { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime LastUpdated { get; set; }

        public Boolean Active { get; set; }

        public ICollection<String> Claims { get; set; }
    }
}
