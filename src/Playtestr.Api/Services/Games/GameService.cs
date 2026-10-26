using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Extensions;
using Playtestr.Api.Mappings;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Games;

public class GameService(AppDbContext context) : IGameService
{
    public async Task<IReadOnlyList<GameResponse>> GetAllAsync() =>
        await context.Games
            .Select(g => g.ToResponse())
            .ToListAsync();

    public async Task<GameResponse> AddAsync(CreateGameRequest request, int ownerId)
    {
        var newGame = new Game
        {
            Name = request.Name,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            OwnerId = ownerId
        };

        context.Games.Add(newGame);
        await context.SaveChangesAsync();
        return newGame.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var game = await context.Games.FindAsync(id);
        if (game is null) return false;
        context.Games.Remove(game);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<GameResponse?> GetByIdAsync(int id)
    {
        var game = await context.Games.FindAsync(id);
        return game?.ToResponse();
    }

    public async Task<GameResponse?> UpdateAsync(int id, CreateGameRequest request)
    {
        var game = await context.Games.FindAsync(id);
        if (game is null) return null;

        game.Name = request.Name;
        game.Description = request.Description;
        game.ReleaseYear = request.ReleaseYear;

        await context.SaveChangesAsync();
        return game.ToResponse();
    }
}
