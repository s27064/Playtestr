using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Mappings;

public static class PlaytestMappings
{
    public static PlaytestResponse ToResponse(this Playtest p) =>
        new(p.Id, p.Title, p.Description, p.CreatedAt, p.GameId);
}
