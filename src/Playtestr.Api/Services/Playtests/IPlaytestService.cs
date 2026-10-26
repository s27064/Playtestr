using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Playtests;

public interface IPlaytestService
{
    Task<IReadOnlyList<PlaytestResponse>> GetAllAsync();
    Task<PlaytestResponse?> GetByIdAsync(int id);
    Task<IReadOnlyList<PlaytestResponse>?> GetByGameIdAsync(int gameId);
    Task<OperationResult<PlaytestResponse>> AddAsync(int gameId, int userId, CreatePlaytestRequest request);
    Task<OperationResult<PlaytestResponse>> UpdateAsync(int id, int userId, CreatePlaytestRequest request);
    Task<OperationStatus> DeleteAsync(int id, int userId);
}