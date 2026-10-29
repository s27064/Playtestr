using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.PlaytestSummaries;

public interface IPlaytestSummaryService
{
    Task<OperationResult<PlaytestSummaryResponse>> GetPlaytestSummaryAsync(int playtestId, int userId);
}
