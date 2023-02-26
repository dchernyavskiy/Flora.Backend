using Flora.Identity.Data;
using Flora.Identity.Exceptions;
using Flora.Identity.Interfaces;
using IdentityServer4.Services;
using Flora.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flora.Identity.Controllers;

[Route("[controller]/[action]")]
public class AuthController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly AuthDbContext _context;
    private readonly IEmailSender _emailSender;

    public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager, IIdentityServerInteractionService interactionService, AuthDbContext context, IEmailSender emailSender)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _interactionService = interactionService;
        _context = context;
        _emailSender = emailSender;
    }

    [HttpGet]
    public async Task<ActionResult<AppUserDto>> GetData()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            throw new NotFoundException("User");
        var dto = new AppUserDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        return dto;
    }

    [HttpPut]
    public async Task<ActionResult<bool>> PutData([FromBody]AppUserDto userDto)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            throw new NotFoundException("User");

        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        if (user.Email != userDto.Email)
        {
            await ChangeEmailAsync(user, userDto.Email);
        }
        user.PhoneNumber = userDto.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return true;
        return false;
    }

    private async Task ChangeEmailAsync(AppUser user, string newEmail)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var log = new EmailChangeLog()
        {
            Id = Guid.NewGuid(),
            OldEmail = user.Email,
            NewEmail = newEmail,
            DateOfRequest = DateTime.UtcNow,
            AppUser = user,
            Token = token
        };
        await _context.EmailChangeLogs.AddRangeAsync(log);
        await _context.SaveChangesAsync(CancellationToken.None);
        
        var callbackUrl = Url.Action(nameof(ConfirmEmail),
            "Auth",
            new { userId = user.Id, token = token },
            HttpContext.Request.Scheme
            );
        
        var message = $"<!DOCTYPE html><html><head><meta charset=\"UTF-8\"/><meta name=\"viewport\" content=\"width=device-width\" /><script src=\"https://cdn.tailwindcss.com\"></script><title>Confirm Email</title></head><body><p>To confirm of changing email <a href=\"{callbackUrl}\">click here.</a></p><a href=\"https://www.example.com\">Click here to visit Example.com</a></body></html>";
        var result =  await _emailSender.SendEmailAsync(message, $"{user.FirstName} {user.LastName}", newEmail, "Email confirmation");
    }

    [HttpGet]
    public async Task<ActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException("User");
        
        var log = await _context.EmailChangeLogs
            .FirstAsync(x => x.Token == token);
        if (log == null) throw new NotFoundException("Log");
        
        user.Email = log.NewEmail;
        user.EmailConfirmed = true;
        
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return Ok();
        
        return BadRequest();
    }

    [HttpGet]
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
        if (role == null) throw new NotFoundException(vm.Role);

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