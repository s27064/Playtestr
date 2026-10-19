using Playtestr.Api.Models;

namespace Playtestr.Api.Dtos;

public record class QuestionResponse
(
    int Id,
    string Text,
    QuestionType Type,
    int PlaytestId
);
