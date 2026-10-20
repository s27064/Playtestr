using System;
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
        var submission = await context.Submissions.FindAsync(id);
        return submission is null ? null : submission.ToResponse();
    }

    public async Task<IReadOnlyList<SubmissionResponse>?> GetByPlaytestIdAsync(int playtestId)
    {
        if (!await context.Playtests.AnyAsync(p => p.Id == playtestId)) return null;
        return await context.Submissions
            .Where(s => s.PlaytestId == playtestId)
            .Select(s => s.ToResponse())
            .ToListAsync();
    }

    public async Task<SubmitResult> SubmitAsync(int playtestId, CreateSubmissionRequest request)
    {
        if (!await context.Playtests.AnyAsync(p => p.Id == playtestId))
            return new(SubmitStatus.PlaytestNotFound, Error: "Playtest not found");

        var questions = await context.Questions
            .Where(q => q.PlaytestId == playtestId)
            .ToDictionaryAsync(q => q.Id);

        if (request.Answers.DistinctBy(a => a.QuestionId).Count() != request.Answers.Count)
            return new(SubmitStatus.InvalidAnswers, Error: "Each question can only be answered once");

        foreach (var a in request.Answers)
        {
            if (!questions.TryGetValue(a.QuestionId, out var q))
                return new(SubmitStatus.InvalidAnswers, Error: $"Question {a.QuestionId} is not part of this playtest");

            var valid = q.Type == QuestionType.Rating
                ? a.Rating is not null && a.Text is null
                : !string.IsNullOrWhiteSpace(a.Text) && a.Rating is null;

            if (!valid)
                return new(SubmitStatus.InvalidAnswers, Error: $"Invalid answer for question {q.Id}.");
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
