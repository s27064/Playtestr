using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Models;

public class Game
{
    public int Id { get; set; }
    [MaxLength(100)] public string Name { get; set; } = "";
    [MaxLength(255)] public string Description { get; set; } = "";
    public int ReleaseYear { get; set; }

    public List<Playtest> Playtests { get; set; } = [];

    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}