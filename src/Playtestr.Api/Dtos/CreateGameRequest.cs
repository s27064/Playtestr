namespace Playtestr.Api.Dtos;

public record class CreateGameRequest(string Name, string Description, int ReleaseYear);