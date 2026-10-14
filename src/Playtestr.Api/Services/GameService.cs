using Playtestr.Api.Models;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services;

public class GameService
{
    private readonly List<Game> _games =
    [
        new(0, "Cyberpunk", "COOL SHOOTA", 2020),
        new(1, "Witcher 3", "Old guy with sword", 2015),
        new(2, "Counter Strike", "Best shoota", 2010)
    ];
    private int _nextId = 3;

    public IReadOnlyList<Game> GetAll() => _games;

    public Game? GetById(int Id) => _games.FirstOrDefault(g => g.Id == Id);

    public Game Add(CreateGameRequest request)
    {
        var newGame = new Game(_nextId++, request.Name, request.Description, request.ReleaseYear);
        _games.Add(newGame);
        return newGame;
    }

    public Game? Update(int id, CreateGameRequest request)
    {
        var index = _games.FindIndex(g => g.Id == id);
        if (index == -1) return null; // Not Found

        var updated = new Game(id, request.Name, request.Description, request.ReleaseYear);
        _games[index] = updated;
        return updated;
    }

    public bool Delete(int id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game is null) return false;
        return _games.Remove(game);
    }
}
