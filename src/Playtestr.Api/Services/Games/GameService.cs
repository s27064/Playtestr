using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Mappings;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Games;

public class GameService(AppDbContext context) : IGameService
{
    public async Task<IReadOnlyList<GameResponse>> GetAllAsync() =>
        await context.Games
            .Select(g => g.ToResponse())
            .ToListAsync();

    public async Task<GameResponse> AddAsync(CreateGameRequest request, int userId)
    {
        var newGame = new Game
        {
            Name = request.Name,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            OwnerId = userId
        };

        context.Games.Add(newGame);
        await context.SaveChangesAsync();
        return newGame.ToResponse();
    }

    public async Task<OperationStatus> DeleteAsync(int id, int userId)
    {
        var game = await context.Games.FindAsync(id);
        if (game is null) return OperationStatus.NotFound;
        if (game.OwnerId != userId) return OperationStatus.Forbidden;
        context.Games.Remove(game);
        await context.SaveChangesAsync();
        return OperationStatus.Success;
    }

    public async Task<GameResponse?> GetByIdAsync(int id)
    {
        var game = await context.Games.FindAsync(id);
        return game?.ToResponse();
    }

    public async Task<OperationResult<GameResponse>> UpdateAsync(int id, int userId, CreateGameRequest request)
    {
        var game = await context.Games.FindAsync(id);
        if (game is null) return new OperationResult<GameResponse>(OperationStatus.NotFound);
        if (game.OwnerId != userId) return new OperationResult<GameResponse>(OperationStatus.Forbidden);

        game.Name = request.Name;
        game.Description = request.Description;
        game.ReleaseYear = request.ReleaseYear;

        await context.SaveChangesAsync();
        return new OperationResult<GameResponse>(OperationStatus.Success, game.ToResponse());
    }
}
