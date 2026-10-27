using System.Security.Claims;
using Playtestr.Api.Dtos;
using Playtestr.Api.Services.Submissions;
using Playtestr.Api.Extensions;
using Playtestr.Api.Services;

namespace Playtestr.Api.Endpoints;

public static class SubmissionEndpoints
{
    public static void MapSubmissionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/submissions/{id}", async (int id, ISubmissionService service, ClaimsPrincipal user) =>
        {
            var result = await service.GetByIdAsync(id, user.GetUserId());
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem("Submission not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be the submitter or an owner of this game to see this submission", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.Ok(result.Value)
            };
        })
        .RequireAuthorization()
        .WithTags("Submissions");

        app.MapGet("/playtests/{playtestId}/submissions", async (int playtestId, ISubmissionService service, ClaimsPrincipal user) =>
        {
            var result = await service.GetByPlaytestIdAsync(playtestId, user.GetUserId());
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem("Playtest not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be an owner of this game to see its submissions", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.Ok(result.Value)
            };
        })
        .RequireAuthorization()
        .WithTags("Submissions");

        app.MapPost("/playtests/{playtestId}/submissions", async (int playtestId, CreateSubmissionRequest request, ClaimsPrincipal user, ISubmissionService service) =>
        {
            var result = await service.SubmitAsync(playtestId, user.GetUserId(), request);
            return result.Status switch
            {
                OperationStatus.NotFound => Results.Problem(result.Error, statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Invalid => Results.Problem(result.Error, statusCode: StatusCodes.Status400BadRequest),
                OperationStatus.Conflict => Results.Problem(result.Error, statusCode: StatusCodes.Status409Conflict),
                _ => Results.Created($"/submissions/{result.Value!.Id}", result.Value)
            };
        })
        .RequireAuthorization()
        .WithTags("Submissions");

        app.MapDelete("/submissions/{id}", async (int id, ISubmissionService service, ClaimsPrincipal user) =>
        {
            var status = await service.DeleteAsync(id, user.GetUserId());
            return status switch
            {
                OperationStatus.NotFound => Results.Problem("Submission not found", statusCode: StatusCodes.Status404NotFound),
                OperationStatus.Forbidden => Results.Problem("You must be the submitter or an owner of this game to delete this submission", statusCode: StatusCodes.Status403Forbidden),
                _ => Results.NoContent()
            };
        })
        .RequireAuthorization()
        .WithTags("Submissions");
    }
}
