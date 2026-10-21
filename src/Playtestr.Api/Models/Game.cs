namespace Playtestr.Api.Models;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public int ReleaseYear { get; set; }

    public List<Playtest> Playtests { get; set; } = [];
}