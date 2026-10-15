using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services;

public class GameService(AppDbContext context) : IGameService
{
    public async Task<IReadOnlyList<Game>> GetAllAsync() => await context.Games.ToListAsync();

    public async Task<Game> AddAsync(CreateGameRequest request)
    {
        var newGame = new Game
        {
            Name = request.Name,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear
        };

        context.Games.Add(newGame);
        await context.SaveChangesAsync();
        return newGame;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var game = await context.Games.FindAsync(id);
        if (game is null) return false;
        context.Games.Remove(game);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Game?> GetByIdAsync(int id) => await context.Games.FindAsync(id);

    public async Task<Game?> UpdateAsync(int id, CreateGameRequest request)
    {
        var game = await context.Games.FindAsync(id);
        if (game is null) return null;

        game.Name = request.Name;
        game.Description = request.Description;
        game.ReleaseYear = request.ReleaseYear;

        await context.SaveChangesAsync();
        return game;
    }
}
