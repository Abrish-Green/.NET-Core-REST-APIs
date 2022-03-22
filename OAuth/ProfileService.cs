using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using OAuthApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OAuthApp.OAuth
{
    public class ProfileService : IProfileService
    {
        private readonly OAuthDBContext _context;

        public ProfileService(OAuthDBContext context)
        {
            _context = context;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext profileContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(profileContext.Subject.Identity.Name))
                {
                    var user = await _context.AuthUser
                        .SingleOrDefaultAsync(m => m.Username == profileContext.Subject.Identity.Name);

                    if (user != null)
                    {
                        var profile = await _context.AuthUserProfile.SingleOrDefaultAsync(m => m.Username == user.Username);
                        //var claims = ResourceOwnerPasswordValidator.GetUserClaims(user, profile);

                        //// adding for roles
                        var claims = new List<Claim>();
                        claims.AddRange(ResourceOwnerPasswordValidator.GetUserClaims(user, profile));
                        var userRoles = await Task.Run(() =>
                               _context.AuthUserRole.Where(o => o.Username == user.Username));

                        AuthUserRole[] roles = userRoles.ToArray();
                        foreach (var role in roles)
                            claims.Add(new Claim(ClaimTypes.Role, role.Rolename));
                        /////////////////////////////////////////////// 

                        profileContext.IssuedClaims = claims.ToList();
                        //profileContext.IssuedClaims = claims.Where(
                        //x => profileContext.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
                else
                {
                    var userId = profileContext.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrEmpty(userId.Value))
                    {
                        var user = await _context.AuthUser
                            .SingleOrDefaultAsync(m => m.Username == userId.Value);

                        if (user != null)
                        {
                            var profile = await _context.AuthUserProfile.SingleOrDefaultAsync(m => m.Username == user.Username);
                            //var claims = ResourceOwnerPasswordValidator.GetUserClaims(user, profile);

                            //// adding for roles
                            var claims = new List<Claim>();
                            claims.AddRange(ResourceOwnerPasswordValidator.GetUserClaims(user, profile));
                            var userRoles = await Task.Run(() =>
                                   _context.AuthUserRole.Where(o => o.Username == user.Username));

                            AuthUserRole[] roles = userRoles.ToArray();
                            foreach (var role in roles)
                                claims.Add(new Claim(ClaimTypes.Role, role.Rolename));
                            /////////////////////////////////////////////// 

                            profileContext.IssuedClaims = claims.ToList();
                            //profileContext.IssuedClaims =claims.Where(x => profileContext.RequestedClaimTypes.Contains(x.Type)).ToList();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id);

                if (!string.IsNullOrEmpty(userId.Value))
                {
                    var user = await _context.AuthUser.SingleOrDefaultAsync
                        (m => m.Username == userId.Value);

                    // set active user                    
                    context.IsActive = user != null;

                    // save active user  into database
                    user.Islogged = user != null;
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
