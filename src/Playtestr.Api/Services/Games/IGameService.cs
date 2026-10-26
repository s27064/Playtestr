using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Games;

public interface IGameService
{
    Task<IReadOnlyList<GameResponse>> GetAllAsync();
    Task<GameResponse?> GetByIdAsync(int id);
    Task<GameResponse> AddAsync(CreateGameRequest request, int ownerId);
    Task<GameResponse?> UpdateAsync(int id, CreateGameRequest request);
    Task<bool> DeleteAsync(int id);
}
