using Playtestr.Api.Models;

namespace Playtestr.Api.Services.Auth;

public interface ITokenService
{
    string CreateToken(User user);
}
