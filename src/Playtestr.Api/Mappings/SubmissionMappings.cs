using System;
using Playtestr.Api.Dtos;
using Playtestr.Api.Models;

namespace Playtestr.Api.Mappings;

public static class SubmissionMappings
{
    public static SubmissionResponse ToResponse(this Submission s) =>
        new(s.Id, s.PlaytestId, s.IsAnonymous ? null : s.User.Username, s.SubmittedAt, s.Answers.Select(a => a.ToResponse()).ToList());
}