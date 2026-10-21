using Playtestr.Api.Services.Results;

namespace Playtestr.Api.Endpoints;

public static class ResultEndpoints
{
    public static void MapResultEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/playtests/{playtestId}/results", async (int playtestId, IResultService service) =>
        {
            var results = await service.GetPlaytestResultsAsync(playtestId);
            return results is null ? Results.NotFound("Playtest not found") : Results.Ok(results);
        }).WithTags("Results");
    }
}
