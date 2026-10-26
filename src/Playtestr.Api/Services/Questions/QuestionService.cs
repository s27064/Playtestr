using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;
using Playtestr.Api.Mappings;

namespace Playtestr.Api.Services.Questions;

public class QuestionService(AppDbContext context) : IQuestionService
{
    public async Task<OperationResult<QuestionResponse>> AddAsync(int playtestId, int userId, CreateQuestionRequest request)
    {
        var playtest = await context.Playtests
            .Include(p => p.Game)
            .FirstOrDefaultAsync(p => p.Id == playtestId);

        if (playtest is null) return new(OperationStatus.NotFound);
        if (playtest.Game.OwnerId != userId) return new(OperationStatus.Forbidden);

        var question = new Question
        {
            Text = request.Text,
            Type = request.Type,
            PlaytestId = playtestId
        };

        context.Questions.Add(question);
        await context.SaveChangesAsync();
        return new(OperationStatus.Success, question.ToResponse());
    }

    public async Task<IReadOnlyList<QuestionResponse>> GetAllAsync() =>
        await context.Questions
            .Select(q => q.ToResponse())
            .ToListAsync();


    public async Task<QuestionResponse?> GetByIdAsync(int id)
    {
        var question = await context.Questions.FindAsync(id);
        return question?.ToResponse();
    }

    public async Task<IReadOnlyList<QuestionResponse>?> GetByPlaytestIdAsync(int playtestId)
    {
        if (!await context.Playtests.AnyAsync(p => p.Id == playtestId)) return null;
        return await context.Questions
            .Where(q => q.PlaytestId == playtestId)
            .Select(q => q.ToResponse())
            .ToListAsync();
    }

    public async Task<OperationStatus> DeleteAsync(int id, int userId)
    {
        var question = await context.Questions
            .Include(q => q.Playtest)
                .ThenInclude(p => p.Game)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question is null) return OperationStatus.NotFound;
        if (question.Playtest.Game.OwnerId != userId) return OperationStatus.Forbidden;

        context.Questions.Remove(question);
        await context.SaveChangesAsync();
        return OperationStatus.Success;
    }

    // the text can always be edited but the type cant change once answers exist
    public async Task<OperationResult<QuestionResponse>> UpdateAsync(int id, int userId, CreateQuestionRequest request)
    {
        var question = await context.Questions
            .Include(q => q.Playtest)
                .ThenInclude(p => p.Game)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question is null) return new(OperationStatus.NotFound);
        if (question.Playtest.Game.OwnerId != userId) return new(OperationStatus.Forbidden);

        var typeChanged = question.Type != request.Type;
        if (typeChanged && await context.Answers.AnyAsync(a => a.QuestionId == id))
            return new(OperationStatus.Conflict);

        question.Text = request.Text;
        question.Type = request.Type;
        await context.SaveChangesAsync();

        return new(OperationStatus.Success, question.ToResponse());
    }
}