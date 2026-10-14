using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services;

public class GameServiceDB(AppDbContext context) : IGameService
{
    public IReadOnlyList<Game> GetAll() => context.Games.ToList();

    public Game Add(CreateGameRequest request)
    {
        var newGame = new Game
        {
            Name = request.Name,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear
        };

        context.Games.Add(newGame);
        context.SaveChanges();
        return newGame;
    }

    public bool Delete(int id)
    {
        var game = context.Games.Find(id);
        if (game is null) return false;
        context.Games.Remove(game);
        context.SaveChanges();
        return true;
    }

    public Game? GetById(int id) => context.Games.Find(id);

    public Game? Update(int id, CreateGameRequest request)
    {
        var game = context.Games.Find(id);
        if (game is null) return null;

        game.Name = request.Name;
        game.Description = request.Description;
        game.ReleaseYear = request.ReleaseYear;

        context.SaveChanges();
        return game;
    }
}
