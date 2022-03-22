using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthApp.OAuth
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(
                    "OAuth2Demo.ReadAccess",
                    "OAuth2Demo API",
                    new List<string> {
                        JwtClaimTypes.Id,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Address,
                        JwtClaimTypes.Role                        
                    }
                ),

                new ApiResource("OAuth2Demo.FullAccess", "OAuth2Demo API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "HTML5 Client",
                    ClientId = "html5Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("mysecretkey".Sha256())
                        //new Secret("mysecretkey")
                    },

                    AllowedScopes = { "OAuth2Demo.ReadAccess" }
                }
            };
        }
    }
}
