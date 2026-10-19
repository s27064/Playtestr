using Playtestr.Api.Dtos;
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
            return game is null ? Results.NotFound() : Results.Ok(game);
        });

        group.MapPost("/", async (CreateGameRequest request, IGameService service) =>
        {
            var game = await service.AddAsync(request);
            return Results.Created($"/games/{game.Id}", game);
        });

        group.MapPut("/{id}", async (int id, CreateGameRequest request, IGameService service) =>
        {
            var game = await service.UpdateAsync(id, request);
            return game is null ? Results.NotFound() : Results.Ok(game);
        });

        group.MapDelete("/{id}", async (int id, IGameService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}