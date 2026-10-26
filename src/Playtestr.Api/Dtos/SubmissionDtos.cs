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

public enum SubmitStatus { Created, PlaytestNotFound, InvalidAnswers, AlreadySubmitted }

public record SubmitResult(SubmitStatus Status, SubmissionResponse? Submission = null, string? Error = null);