using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace SuperSimpleBlobStore.Api.Tests.Infrastructure
{
    public class ApiTestBase
    {
        public Dictionary<string, object>  OwinUserInformation
        {
            get
            {
                var items = new Dictionary<string, object>();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Test User", ClaimValueTypes.String),
                    new Claim(ClaimTypes.NameIdentifier, "44363926-D61B-43C9-B743-34C58F04A356", ClaimValueTypes.String)
                };

                items.Add("server.User", new ClaimsPrincipal(
                    new ClaimsIdentity(
                        claims,
                        "hawk",
                        ClaimTypes.Name,
                        ClaimTypes.Role)
                    )
                    );

                return items;
            }
        }
    }
}
