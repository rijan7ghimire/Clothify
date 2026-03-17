using System.Security.Claims;
using Clothify.Core.Entities;
using Clothify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Profile;

[Authorize]
public class AddressesModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public AddressesModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public List<Address> Addresses { get; set; } = new();
    public string? SuccessMessage { get; set; }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task OnGetAsync()
    {
        var addrs = await _unitOfWork.Repository<Address>().FindAsync(a => a.UserId == UserId);
        Addresses = addrs.ToList();
    }

    public async Task<IActionResult> OnPostAddAsync(string fullName, string streetLine1, string? streetLine2,
        string city, string state, string zipCode, string phone)
    {
        var address = new Address
        {
            UserId = UserId, FullName = fullName, StreetLine1 = streetLine1,
            StreetLine2 = streetLine2, City = city, State = state,
            ZipCode = zipCode, Country = "Nepal", Phone = phone, IsDefault = false
        };
        await _unitOfWork.Repository<Address>().AddAsync(address);
        await _unitOfWork.SaveChangesAsync();
        SuccessMessage = "Address added!";
        await OnGetAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int addressId)
    {
        var addr = await _unitOfWork.Repository<Address>().GetByIdAsync(addressId);
        if (addr != null && addr.UserId == UserId)
        {
            await _unitOfWork.Repository<Address>().DeleteAsync(addr);
            await _unitOfWork.SaveChangesAsync();
        }
        await OnGetAsync();
        return Page();
    }
}
