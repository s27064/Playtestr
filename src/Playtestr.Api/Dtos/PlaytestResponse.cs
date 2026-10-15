using System.ComponentModel.DataAnnotations;

namespace Playtestr.Api.Dtos;

public record PlaytestResponse(
    int Id, 
    string Title, 
    string Description, 
    DateTime CreatedAt, 
    int GameId
    );
