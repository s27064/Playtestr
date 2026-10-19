using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Mappings;

public static class GameMappings
{
    public static GameResponse ToResponse(this Game g) =>
        new(g.Id, g.Name, g.Description, g.ReleaseYear);
}
