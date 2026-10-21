using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Auth;

public class AuthService(AppDbContext context, IPasswordHasher<User> hasher) : IAuthService
{
    public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var username = request.Username.Trim();

        if (await context.Users.AnyAsync(u => u.Email == email)) return new(RegisterStatus.EmailTaken);
        if (await context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower())) return new(RegisterStatus.UsernameTaken);

        var user = new User
        {
            Email = email,
            Username = username
        };
        user.PasswordHash = hasher.HashPassword(user, request.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new(RegisterStatus.Created, new UserResponse(user.Id, user.Email, user.Username));
    }
}
