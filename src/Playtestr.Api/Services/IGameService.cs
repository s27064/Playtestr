using Playtestr.Api.Models;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services;

public interface IGameService
{
    Task<IReadOnlyList<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(int id);
    Task<Game> AddAsync(CreateGameRequest request);
    Task<Game?> UpdateAsync(int id, CreateGameRequest request);
    Task<bool> DeleteAsync(int id);
}
