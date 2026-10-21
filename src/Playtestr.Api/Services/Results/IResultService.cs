using System;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Results;

public interface IResultService
{
    Task<PlaytestResultsResponse?> GetPlaytestResultsAsync(int playtestId);
}
