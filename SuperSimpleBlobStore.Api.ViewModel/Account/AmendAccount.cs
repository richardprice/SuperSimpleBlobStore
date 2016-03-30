using System;
using System.Collections.Generic;
using Attribute = SuperSimpleBlobStore.Common.Attribute;

namespace SuperSimpleBlobStore.Api.ViewModel
{
    public class AmendAccount
    {
        public AmendAccount()
        {
            Result = new Result();
            Claims = new List<string>();
            AllClaims = new List<Attribute>();
        }

        public string UserId { get; set; }
        public Guid AuthenticationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Result Result { get; set; }
        public List<string> Claims { get; set; }

        public List<Attribute> AllClaims { get; set; }
    }
}
