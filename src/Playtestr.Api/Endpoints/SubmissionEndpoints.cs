using System.Security.Claims;
using Playtestr.Api.Dtos;
using Playtestr.Api.Services.Submissions;
using Playtestr.Api.Extensions;

namespace Playtestr.Api.Endpoints;

public static class SubmissionEndpoints
{
    public static void MapSubmissionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/submissions/{id}", async (int id, ISubmissionService service) =>
        {
            var submission = await service.GetByIdAsync(id);
            return submission is null ?
                Results.Problem("Submission not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Ok(submission);
        }).WithTags("Submissions");

        app.MapGet("/playtests/{playtestId}/submissions", async (int playtestId, ISubmissionService service) =>
        {
            var submissions = await service.GetByPlaytestIdAsync(playtestId);
            return submissions is null ?
                Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound) :
                Results.Ok(submissions);
        }).WithTags("Submissions");

        app.MapPost("/playtests/{playtestId}/submissions", async (int playtestId, CreateSubmissionRequest request, ClaimsPrincipal user, ISubmissionService service) =>
        {
            var result = await service.SubmitAsync(playtestId, user.GetUserId(), request);
            return result.Status switch
            {
                SubmitStatus.PlaytestNotFound => Results.Problem(result.Error, statusCode: StatusCodes.Status404NotFound),
                SubmitStatus.InvalidAnswers => Results.Problem(result.Error, statusCode: StatusCodes.Status400BadRequest),
                SubmitStatus.AlreadySubmitted => Results.Problem(result.Error, statusCode: StatusCodes.Status409Conflict),
                _ => Results.Created($"/submissions/{result.Submission!.Id}", result.Submission)
            };
        })
        .RequireAuthorization()
        .WithTags("Submissions");

        app.MapDelete("/submissions/{id}", async (int id, ISubmissionService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ?
                Results.NoContent() :
                Results.Problem("Submission not found", statusCode: StatusCodes.Status404NotFound);
        }).WithTags("Submissions");
    }
}
