using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Dtos;

public record LoginRequest
(
    [Required] string Login, // Can be either an Email or Username
    [Required, MinLength(8), MaxLength(100)] string Password
);

public record LoginResponse(string Token, UserResponse User);

public record RegisterRequest
(
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, MinLength(8), MaxLength(100)] string Password,
    [Required, MaxLength(50), RegularExpression(@"^[a-zA-Z0-9_.-]+$")] string Username
);

public record UserResponse(int Id, string Email, string Username);

public enum RegisterStatus { Created, EmailTaken, UsernameTaken }

public record RegisterResult(RegisterStatus Status, UserResponse? User = null);