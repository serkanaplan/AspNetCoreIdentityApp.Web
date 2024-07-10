using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.ClaimProvider;

public class UserClaimProvider(UserManager<AppUser> userManager) : IClaimsTransformation
{
    private readonly UserManager<AppUser> _userManager = userManager;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {

        var identityUser = principal.Identity as ClaimsIdentity;

        var currentUser = await _userManager.FindByNameAsync(identityUser!.Name!);

        if (String.IsNullOrEmpty(currentUser!.City))
        {
            return principal;
        }

        if (principal.HasClaim(x => x.Type != "city"))
        {
            Claim cityClaim = new("city", currentUser.City);

            identityUser.AddClaim(cityClaim);
        }

        return principal;
    }
}

