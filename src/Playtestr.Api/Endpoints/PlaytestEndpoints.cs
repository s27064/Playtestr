using System.Security.Claims;
using Playtestr.Api.Dtos;
using Playtestr.Api.Extensions;
using Playtestr.Api.Services;
using Playtestr.Api.Services.Playtests;

namespace Playtestr.Api.Endpoints;

public static class PlaytestEndpoints
{
    public static void MapPlaytestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/playtests", async (IPlaytestService service) =>
        {
            return await service.GetAllAsync();
        })
        .WithTags("Playtests");

        app.MapGet("/playtests/{id}", async (int id, IPlaytestService service) =>
        {
            var playtest = await service.GetByIdAsync(id);
            return playtest is null ?
                Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Ok(playtest);
        })
        .WithTags("Playtests");

        app.MapGet("/games/{gameId}/playtests", async (int gameId, IPlaytestService service) =>
        {
            var playtests = await service.GetByGameIdAsync(gameId);
            return playtests is null ?
                Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Ok(playtests);
        })
        .WithTags("Playtests");

        app.MapPost("/games/{gameId}/playtests", async (int gameId, CreatePlaytestRequest request, IPlaytestService service, ClaimsPrincipal user) =>
        {
            var result = await service.AddAsync(gameId, user.GetUserId(), request);
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be an owner of this game to add a playtest to it", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.Created($"/playtests/{result.Value!.Id}", result.Value)
            };
        })
        .RequireAuthorization()
        .WithTags("Playtests");

        app.MapPut("/playtests/{id}", async (int id, CreatePlaytestRequest request, IPlaytestService service, ClaimsPrincipal user) =>
        {
            var result = await service.UpdateAsync(id, user.GetUserId(), request);
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be an owner of this game to modify its playtests", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.Ok(result.Value)
            };
        })
        .RequireAuthorization()
        .WithTags("Playtests");

        app.MapDelete("/playtests/{id}", async (int id, IPlaytestService service, ClaimsPrincipal user) =>
        {
            var status = await service.DeleteAsync(id, user.GetUserId());
            return status switch
            {
                OperationStatus.NotFound => Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be an owner of this game to delete its playtests", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.NoContent()
            };
        })
        .RequireAuthorization()
        .WithTags("Playtests");
    }
}
