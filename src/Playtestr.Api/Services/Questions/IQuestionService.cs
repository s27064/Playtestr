using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Questions;

public interface IQuestionService
{
    Task<IReadOnlyList<QuestionResponse>> GetAllAsync();
    Task<QuestionResponse?> GetByIdAsync(int id);
    Task<IReadOnlyList<QuestionResponse>?> GetByPlaytestIdAsync(int playtestId);
    Task<OperationResult<QuestionResponse>> AddAsync(int playtestId, int userId, CreateQuestionRequest request);
    Task<OperationStatus> DeleteAsync(int id, int userId);
    Task<OperationResult<QuestionResponse>> UpdateAsync(int id, int userId, CreateQuestionRequest request);
}
