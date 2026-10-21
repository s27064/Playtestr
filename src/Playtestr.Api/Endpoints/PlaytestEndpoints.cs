using Playtestr.Api.Dtos;
using Playtestr.Api.Services.Playtests;

namespace Playtestr.Api.Endpoints;

public static class PlaytestEndpoints
{
    public static void MapPlaytestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/playtests", async (IPlaytestService service) =>
        {
            return await service.GetAllAsync();
        }).WithTags("Playtests");

        app.MapGet("/playtests/{id}", async (int id, IPlaytestService service) =>
        {
            var playtest = await service.GetByIdAsync(id);
            return playtest is null ? Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound) : Results.Ok(playtest);
        }).WithTags("Playtests");

        app.MapGet("/games/{gameId}/playtests", async (int gameId, IPlaytestService service) =>
        {
            var playtests = await service.GetByGameIdAsync(gameId);
            return playtests is null ? Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound) : Results.Ok(playtests);
        }).WithTags("Playtests");

        app.MapPost("/games/{gameId}/playtests", async (int gameId, CreatePlaytestRequest request, IPlaytestService service) =>
        {
            var playtest = await service.AddAsync(gameId, request);
            return playtest is null ? Results.Problem("Game not found", statusCode: StatusCodes.Status404NotFound) : Results.Created($"/playtests/{playtest.Id}", playtest);
        }).WithTags("Playtests");

        app.MapPut("/playtests/{id}", async (int id, CreatePlaytestRequest request, IPlaytestService service) =>
        {
            var playtest = await service.UpdateAsync(id, request);
            return playtest is null ? Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound) : Results.Ok(playtest);
        }).WithTags("Playtests");

        app.MapDelete("/playtests/{id}", async (int id, IPlaytestService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound);
        }).WithTags("Playtests");
    }
}
