using System;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;
using Playtestr.Api.Services.Auth;

namespace Playtestr.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/auth").WithTags("Auth");

        auth.MapPost("/register", async (RegisterRequest request, IAuthService service) =>
        {
            var outcome = await service.RegisterAsync(request);
            return outcome.Status switch
            {
                RegisterStatus.UsernameTaken => Results.Problem("Username already taken", statusCode: StatusCodes.Status409Conflict),
                RegisterStatus.EmailTaken => Results.Problem("Email already registered", statusCode: StatusCodes.Status409Conflict),
                _ => Results.Created("/auth/me", outcome.User)
            };
        });
    }
}
