using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Flora.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Flora.Identity.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<AppUser> _userManager;

    public ProfileService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        if (user != null)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var phone = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            var claims = new List<Claim>();
            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }
            claims.Add(new Claim(JwtClaimTypes.PhoneNumber, phone));
            claims.Add(new Claim(JwtClaimTypes.Email, email));
            claims.Add(new Claim(JwtClaimTypes.Id, userId));
            
            context.IssuedClaims.AddRange(claims);   
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}