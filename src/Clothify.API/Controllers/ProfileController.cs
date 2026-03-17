using System.Security.Claims;
using AutoMapper;
using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Mappings;
using Clothify.Core.Entities;
using Clothify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ProfileController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _userManager.FindByIdAsync(UserId);
        return Ok(_mapper.Map<UserResponse>(user));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null) return NotFound();

        if (request.FirstName != null) user.FirstName = request.FirstName;
        if (request.LastName != null) user.LastName = request.LastName;
        if (request.PhoneNumber != null) user.PhoneNumber = request.PhoneNumber;
        if (request.DateOfBirth.HasValue) user.DateOfBirth = request.DateOfBirth;
        if (request.Gender != null) user.Gender = request.Gender;

        await _userManager.UpdateAsync(user);
        return Ok(_mapper.Map<UserResponse>(user));
    }

    [HttpGet("addresses")]
    public async Task<IActionResult> GetAddresses()
    {
        var addresses = await _unitOfWork.Repository<Address>().FindAsync(a => a.UserId == UserId);
        return Ok(_mapper.Map<List<AddressResponse>>(addresses));
    }

    [HttpPost("addresses")]
    public async Task<IActionResult> AddAddress([FromBody] AddressRequest request)
    {
        var address = new Address
        {
            UserId = UserId,
            FullName = request.FullName,
            StreetLine1 = request.StreetLine1,
            StreetLine2 = request.StreetLine2,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            Country = request.Country,
            Phone = request.Phone,
            IsDefault = request.IsDefault
        };

        await _unitOfWork.Repository<Address>().AddAsync(address);
        await _unitOfWork.SaveChangesAsync();
        return Created("", _mapper.Map<AddressResponse>(address));
    }

    [HttpDelete("addresses/{id:int}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        var address = await _unitOfWork.Repository<Address>().GetByIdAsync(id);
        if (address == null || address.UserId != UserId) return NotFound();
        await _unitOfWork.Repository<Address>().DeleteAsync(address);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
