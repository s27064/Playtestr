using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Playtestr.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user) =>
        int.Parse(user.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
}