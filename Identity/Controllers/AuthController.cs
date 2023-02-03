using IdentityServer4.Services;
using Flora.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flora.Identity.Controllers;

[Route("[controller]/[action]")]
public class AuthController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IIdentityServerInteractionService _interactionService;

    public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager, IIdentityServerInteractionService interactionService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _interactionService = interactionService;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Login(string returnUrl = "https://localhost:5001")
    {
        var vm = new LoginViewModel()
        {
            ReturnUrl = returnUrl
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await _userManager.FindByEmailAsync(vm.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found");
            return View(vm);
        }

        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, true, false);
        if (result.Succeeded)
            return Redirect(vm.ReturnUrl!);
        
        ModelState.AddModelError(string.Empty, "Login Error");
        return View(vm);
    }

    [HttpGet]
    public IActionResult Register(string returnUrl)
    {
        var vm = new RegisterViewModel()
        {
            ReturnUrl = returnUrl
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = new AppUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = vm.Email.Split('@', StringSplitOptions.TrimEntries)[0],
            Email = vm.Email,
            EmailConfirmed = false
        };
        _ = await _userManager.CreateAsync(user, vm.Password);

        var role = await _roleManager.FindByNameAsync(vm.Role);
        if (role == null) throw new RoleNotFoundException(vm.Role);

        var result = await _userManager.AddToRoleAsync(user, role.Name!);
        if (result.Succeeded)
        {
            return Redirect(vm.ReturnUrl!);
        }

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        await _signInManager.SignOutAsync();
        var request = await _interactionService.GetLogoutContextAsync(logoutId);
        return Redirect(request.PostLogoutRedirectUri);
    }
}

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string role) : base($"Role \"{role}\" was not found.")
    { }
}