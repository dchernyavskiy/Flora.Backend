using Flora.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Flora.Identity.Data;

public class AuthDbContextInitializer
{
    private readonly ILogger<AuthDbContextInitializer> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AuthDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public AuthDbContextInitializer(ILogger<AuthDbContextInitializer> logger,
        RoleManager<IdentityRole> roleManager, AuthDbContext context, UserManager<AppUser> userManager)
    {
        _logger = logger;
        _roleManager = roleManager;
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        try
        {
            await _context.Database.EnsureDeletedAsync();
            if (await _context.Database.EnsureCreatedAsync())
            {
                await TrySeedAsync();
                _logger.LogDebug("Database seeded successfully");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while seeding the database");
        }
    }

    private async Task TrySeedAsync()
    {
        await SeedRolesAsync();
        await SeedUsersAsync();
    }

    private const string BuyerRoleName = "Buyer";
    private const string AdministratorRoleName = "Administrator";

    private async Task SeedRolesAsync()
    {
        var buyer = new IdentityRole() { Name = BuyerRoleName };
        var systemAdministrator = new IdentityRole() { Name = AdministratorRoleName };

        await _roleManager.CreateAsync(buyer);
        await _roleManager.CreateAsync(systemAdministrator);
    }

    private const string Password = "1234";

    private async Task SeedUsersAsync()
    {
        await SeedAdminAsync();
        await SeedBuyerAsync();
    }

    private async Task SeedAdminAsync()
    {
        var user = new AppUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "sysadmin",
            Email = "admin@mail.com",
            EmailConfirmed = true,
        };
        var result = await _userManager.CreateAsync(user, Password);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, AdministratorRoleName);
    }

    private async Task SeedBuyerAsync()
    {
        var user = new AppUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "candidate",
            Email = "candidate@mail.com",
            EmailConfirmed = true,
        };
        var result = await _userManager.CreateAsync(user, Password);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, BuyerRoleName);
    }
}