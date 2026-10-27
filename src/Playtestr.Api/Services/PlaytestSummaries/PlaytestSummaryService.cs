using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.PlaytestSummaries;

public class PlaytestSummaryService(AppDbContext context) : IPlaytestSummaryService
{
    public async Task<OperationResult<PlaytestSummaryResponse>> GetPlaytestSummaryAsync(int playtestId, int userId)
    {
        var playtest = await context.Playtests
            .Include(p => p.Game)
            .FirstOrDefaultAsync(p => p.Id == playtestId);

        if (playtest is null) return new(OperationStatus.NotFound, Error: "Playtest not found");
        if (playtest.Game.OwnerId != userId) return new(OperationStatus.Forbidden, Error: "Only an owner of the game can see the summary of its playtest");

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

        return new(OperationStatus.Success, playtestSummary);
    }
}
