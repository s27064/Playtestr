using Playtestr.Api.Services.PlaytestSummaries;

namespace Playtestr.Api.Endpoints;

public static class PlaytestSummaryEndpoints
{
    public static void MapPlaytestSummaryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/playtests/{playtestId}/results", async (int playtestId, IPlaytestSummaryService service) =>
        {
            var summary = await service.GetPlaytestSummaryAsync(playtestId);
            return summary is null ?
                Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Ok(summary);
        }).WithTags("Results");
    }
}
