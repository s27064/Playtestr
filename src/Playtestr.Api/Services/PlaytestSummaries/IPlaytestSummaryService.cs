using System;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.PlaytestSummaries;

public interface IPlaytestSummaryService
{
    Task<PlaytestSummaryResponse?> GetPlaytestSummaryAsync(int playtestId);
}
