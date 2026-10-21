using System;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Auth;

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(RegisterRequest request);
}
