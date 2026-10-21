using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Results;

public class ResultService(AppDbContext context) : IResultService
{
    public async Task<PlaytestResultsResponse?> GetPlaytestResultsAsync(int playtestId)
    {
        var playtest = await context.Playtests.FindAsync(playtestId);
        if (playtest is null) return null;

        var questions = await context.Questions
            .Where(q => q.PlaytestId == playtestId)
            .Include(q => q.Answers)
            .ToListAsync();

        var questionResults = new List<QuestionResult>();
        foreach (var q in questions)
        {
            var averageRating = q.Answers.Average(a => a.Rating) 
                is double avg ? Math.Round(avg, 2) : (double?)null;
            List<string> textAnswers = q.Answers
            .Where(a => !string.IsNullOrWhiteSpace(a.Text))
            .Select(a => a.Text!)
            .ToList();

            var result = new QuestionResult
            (
                q.Id,
                q.Text,
                q.Type,
                averageRating,
                q.Answers.Count,
                textAnswers
            );
            questionResults.Add(result);
        }

        var submissionCount = await context.Submissions.CountAsync(s => s.PlaytestId == playtestId);
        var playtestResult = new PlaytestResultsResponse
        (
            playtestId,
            playtest.Title,
            submissionCount,
            questionResults
        );

        return playtestResult;
    }
}
