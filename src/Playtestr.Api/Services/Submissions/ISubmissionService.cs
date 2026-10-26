using Playtestr.Api.Dtos;

namespace Playtestr.Api.Services.Submissions;

public interface ISubmissionService
{
    Task<SubmitResult> SubmitAsync(int playtestId, int userId, CreateSubmissionRequest request);
    Task<SubmissionResponse?> GetByIdAsync(int id);
    Task<IReadOnlyList<SubmissionResponse>?> GetByPlaytestIdAsync(int playtestId);
    Task<bool> DeleteAsync(int id);
}
