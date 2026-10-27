namespace Playtestr.Api.Dtos;

public record CreateSubmissionRequest
(
    bool IsAnonymous,
    List<CreateAnswerRequest> Answers
);

public record SubmissionResponse
(
    int Id,
    int PlaytestId,
    string? TesterName,
    DateTime SubmittedAt,
    List<AnswerResponse> Answers
);