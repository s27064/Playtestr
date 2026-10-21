using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Models;

public class Question
{
    public int Id { get; set; }
    [MaxLength(255)] public string Text { get; set; } = "";
    public QuestionType Type { get; set; }

    public int PlaytestId { get; set; }
    public Playtest Playtest { get; set; } = null!;

    public List<Answer> Answers { get; set; } = [];
}
