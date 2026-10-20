using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Dtos;

public record CreateAnswerRequest
(
    int QuestionId,
    [Range(1, 5)] int? Rating,
    string? Text
);

public record AnswerResponse
(
    int Id,
    int QuestionId,
    int? Rating,
    string? Text
);