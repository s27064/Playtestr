using Playtestr.Api.Models;

namespace Playtestr.Api.Dtos;

public record PlaytestResultsResponse 
(
    int PlaytestId,
    string Title,
    int SubmissionCount,
    List<QuestionResult> QuestionResults
);

public record QuestionResult 
(
    int QuestionId,
    string Text,
    QuestionType Type,
    double? AverageRating,
    int AnswerCount,
    List<string> TextAnswers
);