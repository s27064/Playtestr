using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Dtos;

public record CreatePlaytestRequest(
    [Required, MaxLength(100)] string Title,
    [Required, MaxLength(255)] string Description
);

public record PlaytestResponse(
    int Id,
    string Title,
    string Description,
    DateTime CreatedAt,
    int GameId
);
