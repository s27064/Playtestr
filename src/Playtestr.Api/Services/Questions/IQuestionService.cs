using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Questions;

public interface IQuestionService
{
    Task<IReadOnlyList<QuestionResponse>> GetAllAsync();
    Task<QuestionResponse?> GetByIdAsync(int id);
    Task<IReadOnlyList<QuestionResponse>?> GetByPlaytestIdAsync(int playtestId);
    Task<QuestionResponse?> AddAsync(int playtestId, CreateQuestionRequest request);
    Task<bool> DeleteAsync(int id);
    Task<UpdateQuestionResult> UpdateAsync(int id, CreateQuestionRequest request);
}
