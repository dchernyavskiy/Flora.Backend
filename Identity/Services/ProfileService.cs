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
            var userId = await _userManager.GetUserIdAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var phone = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            var claims = new List<Claim>();
            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }
            claims.Add(new Claim(JwtClaimTypes.Id, userId));
            claims.Add(new Claim(JwtClaimTypes.PhoneNumber, phone ?? string.Empty));
            claims.Add(new Claim(JwtClaimTypes.Email, email ?? string.Empty));
            claims.Add(new Claim(nameof(user.FirstName), user.FirstName));
            claims.Add(new Claim(nameof(user.LastName), user.LastName));
            
            context.IssuedClaims.AddRange(claims);   
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}