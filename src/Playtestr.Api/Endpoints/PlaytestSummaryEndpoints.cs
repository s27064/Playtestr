using System.Security.Claims;
using Playtestr.Api.Extensions;
using Playtestr.Api.Services;
using Playtestr.Api.Services.PlaytestSummaries;

namespace Playtestr.Api.Endpoints;

public static class PlaytestSummaryEndpoints
{
    public static void MapPlaytestSummaryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/playtests/{playtestId}/results", async (int playtestId, IPlaytestSummaryService service, ClaimsPrincipal user) =>
        {
            var result = await service.GetPlaytestSummaryAsync(playtestId, user.GetUserId());
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem(result.Error, statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem(result.Error, statusCode: StatusCodes.Status403Forbidden),
                _ => Results.Ok(result.Value)
            };
        })
        .RequireAuthorization()
        .WithTags("Results");
    }
}
