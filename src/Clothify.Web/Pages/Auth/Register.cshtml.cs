using Clothify.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty] public string FullName { get; set; } = string.Empty;
    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string PhoneNumber { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public string ConfirmPassword { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Password != ConfirmPassword)
        {
            Errors.Add("Passwords do not match");
            return Page();
        }

        var names = FullName.Split(' ', 2);
        var user = new ApplicationUser
        {
            FirstName = names[0],
            LastName = names.Length > 1 ? names[1] : "",
            Email = Email,
            UserName = Email,
            PhoneNumber = PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, Password);
        if (!result.Succeeded)
        {
            Errors = result.Errors.Select(e => e.Description).ToList();
            return Page();
        }

        await _userManager.AddToRoleAsync(user, "Customer");
        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToPage("/Home/Index");
    }
}
