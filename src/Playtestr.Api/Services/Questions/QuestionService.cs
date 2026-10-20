using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;
using Playtestr.Api.Mappings;

namespace Playtestr.Api.Services.Questions;

public class QuestionService(AppDbContext context) : IQuestionService
{
    public async Task<QuestionResponse?> AddAsync(int playtestId, CreateQuestionRequest request)
    {
        if (!await context.Playtests.AnyAsync(p => p.Id == playtestId)) return null;
        
        var question = new Question
        {
            Text = request.Text,
            Type = request.Type,
            PlaytestId = playtestId
        };

        context.Questions.Add(question);
        await context.SaveChangesAsync();
        return question.ToResponse();
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

    public async Task<bool> DeleteAsync(int id)
    {
        var question = await context.Questions.FindAsync(id);
        if (question is null) return false;
        context.Questions.Remove(question);
        await context.SaveChangesAsync();
        return true;
    }

    // the text can always be edited but the type cant change once answers exist
    public async Task<UpdateQuestionResult> UpdateAsync(int id, CreateQuestionRequest request)
    {
        var question = await context.Questions.FindAsync(id);
        if (question is null) return new(UpdateQuestionStatus.NotFound);

        var typeChanged = question.Type != request.Type;
        if (typeChanged && await context.Answers.AnyAsync(a => a.QuestionId == id))
            return new(UpdateQuestionStatus.HasAnswers);

        question.Text = request.Text;
        question.Type = request.Type;
        await context.SaveChangesAsync();

        return new(UpdateQuestionStatus.Updated, question.ToResponse());
    }
}