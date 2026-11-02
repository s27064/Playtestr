using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Models;

public class Playtest
{
    public int Id { get; set; }
    [MaxLength(100)] public string Title { get; set; } = "";
    [MaxLength(255)] public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public List<Question> Questions { get; set; } = [];
    public List<Submission> Submissions { get; set; } = [];
}
