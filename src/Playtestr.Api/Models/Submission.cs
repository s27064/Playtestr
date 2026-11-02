using Microsoft.EntityFrameworkCore;

namespace Playtestr.Api.Models;

[Index(nameof(PlaytestId), nameof(UserId), IsUnique = true)]
public class Submission
{
    public int Id { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public bool IsAnonymous { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int PlaytestId { get; set; }
    public Playtest Playtest { get; set; } = null!;

    public List<Answer> Answers { get; set; } = [];
}
