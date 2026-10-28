using System.Security.Claims;
using Playtestr.Api.Dtos;
using Playtestr.Api.Extensions;
using Playtestr.Api.Services;
using Playtestr.Api.Services.Games;

namespace Playtestr.Api.Endpoints;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games").WithTags("Games");

        group.MapGet("/", async (IGameService service) => await service.GetAllAsync());

        group.MapGet("/{id}", async (int id, IGameService service) =>
        {
            var game = await service.GetByIdAsync(id);
            return game is null ?
                Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Ok(game);
        });

        group.MapPost("/", async (CreateGameRequest request, IGameService service, ClaimsPrincipal user) =>
        {
            var game = await service.AddAsync(request, user.GetUserId());
            return Results.Created($"/games/{game.Id}", game);
        })
        .RequireAuthorization();

        group.MapPut("/{id}", async (int id, CreateGameRequest request, IGameService service, ClaimsPrincipal user) =>
        {
            var result = await service.UpdateAsync(id, user.GetUserId(), request);
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You are not the owner of this game", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.Ok(result.Value)
            };
        })
        .RequireAuthorization();

        group.MapDelete("/{id}", async (int id, IGameService service, ClaimsPrincipal user) =>
        {
            var result = await service.DeleteAsync(id, user.GetUserId());
            return result switch
            {
                OperationStatus.NotFound => Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You are not the owner of this game", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.NoContent()
            };
        })
        .RequireAuthorization();
    }
}