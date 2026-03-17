namespace Clothify.Application.DTOs.Response;

public class AuthResponse
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? Expiration { get; set; }
    public UserResponse? User { get; set; }
    public List<string> Errors { get; set; } = new();
}
