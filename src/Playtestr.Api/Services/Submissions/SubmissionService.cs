using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Mappings;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Submissions;

public class SubmissionService(AppDbContext context) : ISubmissionService
{
    public async Task<bool> DeleteAsync(int id)
    {
        var submission = await context.Submissions.FindAsync(id);
        if (submission is null) return false;
        context.Submissions.Remove(submission);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<SubmissionResponse?> GetByIdAsync(int id)
    {
        var submission = await context.Submissions
            .Include(s => s.Answers)
            .FirstOrDefaultAsync(s => s.Id == id);
        return submission is null ? null : submission.ToResponse();
    }

    public async Task<IReadOnlyList<SubmissionResponse>?> GetByPlaytestIdAsync(int playtestId)
    {
        if (!await context.Playtests.AnyAsync(p => p.Id == playtestId)) return null;
        var submissions = await context.Submissions
            .Where(s => s.PlaytestId == playtestId)
            .Include(s => s.Answers)
            .ToListAsync();
        return submissions.Select(s => s.ToResponse()).ToList();
    }

    // 1. check that playtest exists
    // 2. load the playtest's questions
    // 3. for each answer check that its QuestionId is one of those questions. A rating questions has a Rating, a Text has text
    // 4. if anything fails return InvalidAnswers with a message
    public async Task<SubmitResult> SubmitAsync(int playtestId, CreateSubmissionRequest request)
    {
        if (!await context.Playtests.AnyAsync(p => p.Id == playtestId))
            return new(SubmitStatus.PlaytestNotFound, Error: "Playtest not found");

        var questions = await context.Questions
            .Where(q => q.PlaytestId == playtestId)
            .ToDictionaryAsync(q => q.Id);

        if (request.Answers.DistinctBy(a => a.QuestionId).Count() != request.Answers.Count)
            return new(SubmitStatus.InvalidAnswers, Error: "Each question can only be answered once");

        foreach (var answer in request.Answers)
        {
            if (!questions.TryGetValue(answer.QuestionId, out var question))
                return new(SubmitStatus.InvalidAnswers, Error: $"Question {answer.QuestionId} is not part of this playtest");

            var valid = question.Type == QuestionType.Rating
                ? answer.Rating is not null && answer.Text is null
                : !string.IsNullOrWhiteSpace(answer.Text) && answer.Rating is null;

            if (!valid)
                return new(SubmitStatus.InvalidAnswers, Error: $"Invalid answer for question {question.Id}.");
        }

        var submission = new Submission
        {
            PlaytestId = playtestId,
            TesterName = request.TesterName,
            Answers = request.Answers
                .Select(a => new Answer { QuestionId = a.QuestionId, Rating = a.Rating, Text = a.Text })
                .ToList()
        };

        context.Submissions.Add(submission);
        await context.SaveChangesAsync();

        return new(SubmitStatus.Created, submission.ToResponse());
    }
}
