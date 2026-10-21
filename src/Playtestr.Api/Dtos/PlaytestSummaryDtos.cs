using Playtestr.Api.Models;

namespace Playtestr.Api.Dtos;

public record PlaytestSummaryResponse
(
    int PlaytestId,
    string Title,
    int SubmissionCount,
    List<QuestionSummary> QuestionSummaries
);

public record QuestionSummary
(
    int QuestionId,
    string Text,
    QuestionType Type,
    double? AverageRating,
    int AnswerCount,
    List<string> TextAnswers
);