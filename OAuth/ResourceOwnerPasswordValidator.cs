using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using OAuthApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuthApp.OAuth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly OAuthDBContext _context;

        public ResourceOwnerPasswordValidator(OAuthDBContext context)
        {
            _context = context;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await _context.AuthUser.SingleOrDefaultAsync(m => m.Username == context.UserName);
                if (user != null)
                {
                    if (user.Password == context.Password)
                    {
                        var profile = await _context.AuthUserProfile.SingleOrDefaultAsync(m => m.Username == context.UserName);
                    
                        context.Result = new GrantValidationResult(
                            subject: user.Username,
                            authenticationMethod: "database",
                            claims: GetUserClaims(user, profile));

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                        "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    "User does not exist.");
                return;
            }
            catch (Exception)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    "Invalid username or password");
            }
        }

        public static Claim[] GetUserClaims(AuthUser user, AuthUserProfile profile)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id, user.Username ?? ""),
                new Claim(JwtClaimTypes.Name, profile.Fullname ?? ""),
                new Claim(JwtClaimTypes.Email, profile.Email ?? ""),
                new Claim(JwtClaimTypes.Address, profile.Address ?? "")
            };


        }
    }
}
