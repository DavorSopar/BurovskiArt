using System;
using System.Security.Claims;
using ArtistPortfolio.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ArtistPortfolio.Data
{
	public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> _userManager,
            IOptions<IdentityOptions> _options
            ) : base(_userManager, _options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("FullName",
                user.FirstName + " " + user.LastName
                ));

            return identity;
        }
    }
}

