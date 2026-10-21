using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.PlaytestSummaries;

public class PlaytestSummaryService(AppDbContext context) : IPlaytestSummaryService
{
    public async Task<PlaytestSummaryResponse?> GetPlaytestSummaryAsync(int playtestId)
    {
        var playtest = await context.Playtests.FindAsync(playtestId);
        if (playtest is null) return null;

        var questions = await context.Questions
            .Where(q => q.PlaytestId == playtestId)
            .Include(q => q.Answers)
            .ToListAsync();

        var questionSummaries = new List<QuestionSummary>();
        foreach (var question in questions)
        {
            var averageRating = question.Answers.Average(a => a.Rating)
                is double avg ? Math.Round(avg, 2) : (double?)null;
            List<string> textAnswers = question.Answers
            .Where(a => !string.IsNullOrWhiteSpace(a.Text))
            .Select(a => a.Text!)
            .ToList();

            var questionSummary = new QuestionSummary
            (
                question.Id,
                question.Text,
                question.Type,
                averageRating,
                question.Answers.Count,
                textAnswers
            );
            questionSummaries.Add(questionSummary);
        }

        var submissionCount = await context.Submissions.CountAsync(s => s.PlaytestId == playtestId);
        var playtestSummary = new PlaytestSummaryResponse
        (
            playtestId,
            playtest.Title,
            submissionCount,
            questionSummaries
        );

        return playtestSummary;
    }
}
