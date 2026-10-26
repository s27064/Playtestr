using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        auth.MapPost("/login", async (LoginRequest request, IAuthService service) =>
        {
            var result = await service.LoginAsync(request);
            return result is null ?
                Results.Problem("Invalid login or password", statusCode: StatusCodes.Status401Unauthorized) :
                Results.Ok(result);
        });

        auth.MapGet("/me", (ClaimsPrincipal user) => new
        {
            Id = user.FindFirstValue(JwtRegisteredClaimNames.Sub),
            Email = user.FindFirstValue(JwtRegisteredClaimNames.Email),
            Username = user.FindFirstValue("username")
        })
        .RequireAuthorization();
    }
}
