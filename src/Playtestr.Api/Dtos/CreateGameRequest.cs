using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Dtos;

public record CreateGameRequest(
    [Required, MaxLength(100)] string Name, 
    [Required, MaxLength(255)] string Description, 
    [Range(1950, 2100)] int ReleaseYear
    );