using Playtestr.Api.Dtos;
using Playtestr.Api.Services.Questions;

namespace Playtestr.Api.Endpoints;

public static class QuestionEndpoints
{
    public static void MapQuestionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/questions", async (IQuestionService service) => await service.GetAllAsync())
            .WithTags("Questions");

        app.MapGet("/questions/{id}", async (int id, IQuestionService service) =>
        {
            var question = await service.GetByIdAsync(id);
            return question is null ?
                Results.Problem("Question not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Ok(question);
        }).WithTags("Questions");

        app.MapGet("/playtests/{playtestId}/questions", async (int playtestId, IQuestionService service) =>
        {
            var questions = await service.GetByPlaytestIdAsync(playtestId);
            return questions is null ?
                Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Ok(questions);
        }).WithTags("Questions");

        app.MapPost("/playtests/{playtestId}/questions", async (int playtestId, CreateQuestionRequest request, IQuestionService service) =>
        {
            var question = await service.AddAsync(playtestId, request);
            return question is null ?
                Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Created($"/questions/{question.Id}", question);
        }).WithTags("Questions");

        app.MapPut("/questions/{id}", async (int id, CreateQuestionRequest request, IQuestionService service) =>
        {
            var result = await service.UpdateAsync(id, request);
            return result.Status switch
            {
                UpdateQuestionStatus.NotFound => Results.Problem("Question not found", statusCode: StatusCodes.Status404NotFound),
                UpdateQuestionStatus.HasAnswers => Results.Problem("Cannot change the type of a question that has answers", statusCode: StatusCodes.Status409Conflict),
                _ => Results.Ok(result.Question)
            };
        }).WithTags("Questions");

        app.MapDelete("/questions/{id}", async (int id, IQuestionService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ?
                Results.NoContent() :
                Results.Problem("Question not found", statusCode: StatusCodes.Status404NotFound);
        }).WithTags("Questions");
    }
}
