using System.Security.Claims;
using Playtestr.Api.Dtos;
using Playtestr.Api.Extensions;
using Playtestr.Api.Services;
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

        app.MapPost("/playtests/{playtestId}/questions", async (int playtestId, CreateQuestionRequest request, IQuestionService service, ClaimsPrincipal user) =>
        {
            var result = await service.AddAsync(playtestId, user.GetUserId(), request);
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be an owner of this game to add questions to its playtests", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.Created($"/questions/{result.Value!.Id}", result.Value)
            };
        })
        .RequireAuthorization()
        .WithTags("Questions");

        app.MapPut("/questions/{id}", async (int id, CreateQuestionRequest request, IQuestionService service, ClaimsPrincipal user) =>
        {
            var result = await service.UpdateAsync(id, user.GetUserId(), request);
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem("Question not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be an owner of this game to modify its questions", statusCode: StatusCodes.Status403Forbidden),
                OperationStatus.Conflict => Results.Problem("Cannot change the type of a question that has answers", statusCode: StatusCodes.Status409Conflict),
                _ => Results.Ok(result.Value)
            };
        })
        .RequireAuthorization()
        .WithTags("Questions");

        app.MapDelete("/questions/{id}", async (int id, IQuestionService service, ClaimsPrincipal user) =>
        {
            var status = await service.DeleteAsync(id, user.GetUserId());
            return status switch
            {
                OperationStatus.NotFound => Results.Problem("Question not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be an owner of this game to delete its questions", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.NoContent()
            };
        })
        .RequireAuthorization()
        .WithTags("Questions");
    }
}
