using Playtestr.Api.Models;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services;

public interface IGameService
{
    Task<IReadOnlyList<Game>> GetAll();
    Task<Game?> GetById(int id);
    Task<Game> Add(CreateGameRequest request);
    Task<Game?> Update(int id, CreateGameRequest request);
    Task<bool> Delete(int id);
}
