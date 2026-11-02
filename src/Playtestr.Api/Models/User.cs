using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Playtestr.Api.Models;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    [MaxLength(255)] public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    // citext makes the unique index case-insensitive, so "Alex" and "alex" can't both exist
    [Column(TypeName = "citext")] public string Username { get; set; } = "";
}
