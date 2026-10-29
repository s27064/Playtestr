using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Mappings;

public static class AnswerMappings
{
    public static AnswerResponse ToResponse(this Answer a) =>
        new(a.Id, a.QuestionId, a.Rating, a.Text);
}
