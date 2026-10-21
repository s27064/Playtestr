using Playtestr.Api.Dtos;
using Playtestr.Api.Services.Submissions;

namespace Playtestr.Api.Endpoints;

public static class SubmissionEndpoints
{
    public static void MapSubmissionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/submissions/{id}", async (int id, ISubmissionService service) =>
        {
            var submission = await service.GetByIdAsync(id);
            return submission is null ? Results.NotFound() : Results.Ok(submission);
        }).WithTags("Submissions");

        app.MapGet("/playtests/{playtestId}/submissions", async (int playtestId, ISubmissionService service) =>
        {
            var submissions = await service.GetByPlaytestIdAsync(playtestId);
            return submissions is null ? Results.NotFound() : Results.Ok(submissions);
        }).WithTags("Submissions");

        app.MapPost("/playtests/{playtestId}/submissions", async (int playtestId, CreateSubmissionRequest request, ISubmissionService service) =>
        {
            var result = await service.SubmitAsync(playtestId, request);
            return result.Status switch
            {
                SubmitStatus.PlaytestNotFound => Results.NotFound(result.Error),
                SubmitStatus.InvalidAnswers => Results.BadRequest(result.Error),
                _ => Results.Created($"/submissions/{result.Submission!.Id}", result.Submission)
            };
        }).WithTags("Submissions");

        app.MapDelete("/submissions/{id}", async (int id, ISubmissionService service) =>
        {
            var success = await service.DeleteAsync(id);
            return !success ? Results.NotFound() : Results.NoContent();
        }).WithTags("Submissions");
    }
}
