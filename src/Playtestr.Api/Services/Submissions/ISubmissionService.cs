using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Submissions;

public interface ISubmissionService
{
    Task<OperationResult<SubmissionResponse>> SubmitAsync(int playtestId, int userId, CreateSubmissionRequest request);
    Task<OperationResult<SubmissionResponse>> GetByIdAsync(int id, int userId);
    Task<OperationResult<IReadOnlyList<SubmissionResponse>>> GetByPlaytestIdAsync(int playtestId, int userId);
    Task<OperationStatus> DeleteAsync(int id, int userId);
}
