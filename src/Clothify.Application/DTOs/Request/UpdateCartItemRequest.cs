using System.ComponentModel.DataAnnotations;

namespace Clothify.Application.DTOs.Request;

public class UpdateCartItemRequest
{
    [Required, Range(1, 10)]
    public int Quantity { get; set; }
}
