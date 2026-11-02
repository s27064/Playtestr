using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Playtestr.Api.Models;

[Index(nameof(SubmissionId), nameof(QuestionId), IsUnique = true)]
public class Answer
{
    public int Id { get; set; }
    public int? Rating { get; set; }
    [MaxLength(2000)] public string? Text { get; set; }

    public int SubmissionId { get; set; }
    public Submission Submission { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}
