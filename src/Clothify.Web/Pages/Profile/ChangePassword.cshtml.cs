using System.Security.Claims;
using Clothify.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Profile;

[Authorize]
public class ChangePasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordModel(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    [BindProperty] public string CurrentPassword { get; set; } = "";
    [BindProperty] public string NewPassword { get; set; } = "";
    [BindProperty] public string ConfirmPassword { get; set; } = "";
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (NewPassword != ConfirmPassword)
        {
            ErrorMessage = "New passwords do not match";
            return Page();
        }

        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (user == null) return RedirectToPage("/Auth/Login");

        var result = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
        if (result.Succeeded)
        {
            SuccessMessage = "Password changed successfully!";
            CurrentPassword = ""; NewPassword = ""; ConfirmPassword = "";
        }
        else
        {
            ErrorMessage = string.Join(". ", result.Errors.Select(e => e.Description));
        }
        return Page();
    }
}
