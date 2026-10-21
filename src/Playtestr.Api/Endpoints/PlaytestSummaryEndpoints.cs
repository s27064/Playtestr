using Playtestr.Api.Services.PlaytestSummaries;

namespace Playtestr.Api.Endpoints;

public static class PlaytestSummaryEndpoints
{
    public static void MapPlaytestSummaryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/playtests/{playtestId}/results", async (int playtestId, IPlaytestSummaryService service) =>
        {
            var summary = await service.GetPlaytestSummaryAsync(playtestId);
            return summary is null ? Results.NotFound("Playtest not found") : Results.Ok(summary);
        }).WithTags("Results");
    }
}
