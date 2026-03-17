using System.ComponentModel.DataAnnotations;
using Clothify.Core.Enums;

namespace Clothify.Application.DTOs.Request;

public class CreateCouponRequest
{
    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    public DiscountType DiscountType { get; set; }

    [Required, Range(0.01, 100000)]
    public decimal DiscountValue { get; set; }

    public decimal? MinOrderAmount { get; set; }

    [Required, Range(1, 100000)]
    public int MaxUses { get; set; }

    [Required]
    public DateTime ExpiresAt { get; set; }
}
