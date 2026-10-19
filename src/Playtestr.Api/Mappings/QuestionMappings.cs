using System;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Mappings;

public static class QuestionMappings
{
    public static QuestionResponse ToResponse(this Question q) =>
        new(q.Id, q.Text, q.Type, q.PlaytestId);
}
