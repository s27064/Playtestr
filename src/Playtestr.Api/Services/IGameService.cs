using Playtestr.Api.Models;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services;

public interface IGameService
{
    IReadOnlyList<Game> GetAll();
    Game? GetById(int id);
    Game Add(CreateGameRequest request);
    Game? Update(int id, CreateGameRequest request);
    bool Delete(int id);
}
