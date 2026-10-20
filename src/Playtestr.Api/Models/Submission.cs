namespace Playtestr.Api.Models;

public class Submission
{
    public int Id {get; set;}
    public DateTime SubmittedAt {get; set;} = DateTime.UtcNow;
    public string? TesterName { get; set; } //in case want to stay anonymous is nullable
    
    public int PlaytestId {get; set;}
    public Playtest Playtest {get; set;} = null!;
    
    public List<Answer> Answers {get; set;} = [];
}
