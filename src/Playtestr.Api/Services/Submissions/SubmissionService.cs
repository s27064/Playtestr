using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Mappings;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Submissions;

public class SubmissionService(AppDbContext context) : ISubmissionService
{
    public async Task<OperationStatus> DeleteAsync(int id, int userId)
    {
        var submission = await context.Submissions
            .Include(s => s.Playtest)
                .ThenInclude(p => p.Game)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (submission is null) return OperationStatus.NotFound;

        var isGameOwner = submission.Playtest.Game.OwnerId == userId;
        var isSubmitter = submission.UserId == userId;
        if (!isGameOwner && !isSubmitter) return OperationStatus.Forbidden;

        context.Submissions.Remove(submission);
        await context.SaveChangesAsync();
        return OperationStatus.Success;
    }

    public async Task<OperationResult<SubmissionResponse>> GetByIdAsync(int id, int userId)
    {
        var submission = await context.Submissions
            .Include(s => s.Playtest)
                .ThenInclude(p => p.Game)
            .Include(s => s.Answers)
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (submission is null) return new(OperationStatus.NotFound);

        var isGameOwner = submission.Playtest.Game.OwnerId == userId;
        var isSubmitter = submission.UserId == userId;
        if (!isGameOwner && !isSubmitter) return new(OperationStatus.Forbidden);

        return new(OperationStatus.Success, submission.ToResponse());
    }

    // only the game's owner can see all submissions of a playtest
    public async Task<OperationResult<IReadOnlyList<SubmissionResponse>>> GetByPlaytestIdAsync(int playtestId, int userId)
    {
        var playtest = await context.Playtests
            .Include(p => p.Game)
            .FirstOrDefaultAsync(p => p.Id == playtestId);

        if (playtest is null) return new(OperationStatus.NotFound);
        if (playtest.Game.OwnerId != userId) return new(OperationStatus.Forbidden);

        var submissions = await context.Submissions
            .Where(s => s.PlaytestId == playtestId)
            .Include(s => s.Answers)
            .Include(s => s.User)
            .ToListAsync();

        return new(OperationStatus.Success, submissions.Select(s => s.ToResponse()).ToList());
    }

    public async Task<OperationResult<SubmissionResponse>> SubmitAsync(int playtestId, int userId, CreateSubmissionRequest request)
    {
        if (!await context.Playtests.AnyAsync(p => p.Id == playtestId))
            return new(OperationStatus.NotFound, Error: "Playtest not found");

        var alreadySubmitted = await context.Submissions
           .AnyAsync(s => s.PlaytestId == playtestId && s.UserId == userId);
        if (alreadySubmitted)
            return new(OperationStatus.Conflict, Error: "You have already submitted answers for this playtest.");

        var questions = await context.Questions
            .Where(q => q.PlaytestId == playtestId)
            .ToDictionaryAsync(q => q.Id);

        if (request.Answers.DistinctBy(a => a.QuestionId).Count() != request.Answers.Count)
            return new(OperationStatus.Invalid, Error: "Each question can only be answered once");

        foreach (var answer in request.Answers)
        {
            if (!questions.TryGetValue(answer.QuestionId, out var question))
                return new(OperationStatus.Invalid, Error: $"Question {answer.QuestionId} is not part of this playtest");

            var valid = question.Type == QuestionType.Rating
                ? answer.Rating is not null && answer.Text is null
                : !string.IsNullOrWhiteSpace(answer.Text) && answer.Rating is null;

            if (!valid)
                return new(OperationStatus.Invalid, Error: $"Invalid answer for question {question.Id}.");
        }

        var submission = new Submission
        {
            PlaytestId = playtestId,
            UserId = userId,
            IsAnonymous = request.IsAnonymous,
            Answers = request.Answers
                .Select(a => new Answer { QuestionId = a.QuestionId, Rating = a.Rating, Text = a.Text })
                .ToList()
        };

        context.Submissions.Add(submission);
        await context.SaveChangesAsync();

        // Load the user so the response can show the username
        await context.Entry(submission).Reference(s => s.User).LoadAsync();

        return new(OperationStatus.Success, submission.ToResponse());
    }
}
