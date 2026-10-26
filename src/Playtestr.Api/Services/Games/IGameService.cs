using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Games;

public interface IGameService
{
    Task<IReadOnlyList<GameResponse>> GetAllAsync();
    Task<GameResponse?> GetByIdAsync(int id);
    Task<GameResponse> AddAsync(CreateGameRequest request, int userId);
    Task<OperationResult<GameResponse>> UpdateAsync(int id, int userId, CreateGameRequest request);
    Task<OperationStatus> DeleteAsync(int id, int userId);
}
