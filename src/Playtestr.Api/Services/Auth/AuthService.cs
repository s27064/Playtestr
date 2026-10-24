using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Playtestr.Api.Data;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Auth;

public class AuthService(AppDbContext context, IPasswordHasher<User> hasher, ITokenService tokens) : IAuthService
{
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var login = request.Login.Trim();
        var user = login.Contains('@')
            ? await context.Users.FirstOrDefaultAsync(u => u.Email == login.ToLowerInvariant())
            : await context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == login.ToLower());
        if (user is null) return null;

        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) return null;

        var token = tokens.CreateToken(user);
        return new LoginResponse(token, new UserResponse(user.Id, user.Email, user.Username));
    }

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
