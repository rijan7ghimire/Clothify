using System.ComponentModel.DataAnnotations;

namespace Clothify.Application.DTOs.Request;

public class CreateReviewRequest
{
    [Required, Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(200)]
    public string? Title { get; set; }

    public string? Body { get; set; }
}
