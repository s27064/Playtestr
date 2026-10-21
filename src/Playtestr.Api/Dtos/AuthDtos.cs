using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Dtos;

public record RegisterRequest
(
    [Required, EmailAddress, MaxLength(255)] string Email,
    [Required, MinLength(10), MaxLength(100)] string Password,
    [Required, MaxLength(50)] string Username
);

public record UserResponse(int Id, string Email, string Username);