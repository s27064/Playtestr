using Playtestr.Api.Models;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services;

public class GameService : IGameService
{
    private readonly List<Game> _games =
[
    new() { Id = 0, Name = "Cyberpunk", Description = "COOL SHOOTA", ReleaseYear = 2020 },
    new() { Id = 1, Name = "Witcher 3", Description = "grey hair guy", ReleaseYear = 2015 },
    new() { Id = 2, Name = "Stardew Valley", Description = "cozy farming", ReleaseYear = 2015 }
];
    private int _nextId = 3;

    public IReadOnlyList<Game> GetAll() => _games;

    public Game? GetById(int id) => _games.FirstOrDefault(g => g.Id == id);

    public Game Add(CreateGameRequest request)
    {
        var newGame = new Game
        {
            Id = _nextId++,
            Name = request.Name,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear
        };
        _games.Add(newGame);
        return newGame;
    }

    public Game? Update(int id, CreateGameRequest request)
    {
        var game = GetById(id);
        if (game is null) return null;

        game.Name = request.Name;
        game.Description = request.Description;
        game.ReleaseYear = request.ReleaseYear;
        return game;
    }

    public bool Delete(int id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game is null) return false;
        return _games.Remove(game);
    }
}
