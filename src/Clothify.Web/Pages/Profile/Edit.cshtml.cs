using System.Security.Claims;
using Clothify.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Profile;

[Authorize]
public class EditModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public EditModel(UserManager<ApplicationUser> userManager) => _userManager = userManager;

    [BindProperty] public string FirstName { get; set; } = "";
    [BindProperty] public string LastName { get; set; } = "";
    [BindProperty] public string? PhoneNumber { get; set; }
    [BindProperty] public string? Gender { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task OnGetAsync()
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (user != null)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            PhoneNumber = user.PhoneNumber;
            Gender = user.Gender;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (user == null) return RedirectToPage("/Auth/Login");

        user.FirstName = FirstName;
        user.LastName = LastName;
        user.PhoneNumber = PhoneNumber;
        user.Gender = Gender;
        await _userManager.UpdateAsync(user);
        SuccessMessage = "Profile updated successfully!";
        return Page();
    }
}
