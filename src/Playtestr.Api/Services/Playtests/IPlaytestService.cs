using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Playtests;

public interface IPlaytestService
{
    Task<IReadOnlyList<PlaytestResponse>> GetAllAsync();
    Task<PlaytestResponse?> GetByIdAsync(int id);
    Task<IReadOnlyList<PlaytestResponse>?> GetByGameIdAsync(int gameId);
    Task<PlaytestResponse?> AddAsync(int gameId, CreatePlaytestRequest request);
    Task<PlaytestResponse?> UpdateAsync(int id, CreatePlaytestRequest request);
    Task<bool> DeleteAsync(int id);
}