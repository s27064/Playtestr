using Playtestr.Api.Dtos;
using Playtestr.Api.Services.Questions;

namespace Playtestr.Api.Endpoints;

public static class QuestionEndpoints
{
    public static void MapQuestionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/questions", async (IQuestionService service) => await service.GetAllAsync());

        app.MapGet("/questions/{id}", async (int id, IQuestionService service) =>
        {
            var question = await service.GetByIdAsync(id);
            return question is null ? Results.NotFound() : Results.Ok(question);
        });

        app.MapGet("/playtests/{playtestId}/questions", async (int playtestId, IQuestionService service) =>
        {
            var questions = await service.GetByPlaytestIdAsync(playtestId);
            return questions is null ? Results.NotFound("Playtest not found") : Results.Ok(questions);
        });

        app.MapPost("/playtests/{playtestId}/questions", async (int playtestId, CreateQuestionRequest request, IQuestionService service) =>
        {
            var question = await service.AddAsync(playtestId, request);
            return question is null ? Results.NotFound() : Results.Created($"/questions/{question.Id}", question);
        });

        app.MapPut("/questions/{id}", async (int id, CreateQuestionRequest request, IQuestionService service) =>
        {
            var result = await service.UpdateAsync(id, request);
            return result.Status switch
            {
                UpdateQuestionStatus.NotFound => Results.NotFound(),
                UpdateQuestionStatus.HasAnswers => Results.Conflict("Cannot change the type of a question that has answers"),
                _ => Results.Ok(result.Question)
            };
        });

        app.MapDelete("/questions/{id}", (int id, IQuestionService service) =>
        {
           var question = service.DeleteAsync(id);
           return question is null ? Results.NotFound() : Results.NoContent(); 
        });
    }
}
