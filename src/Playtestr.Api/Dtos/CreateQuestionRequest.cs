using System.ComponentModel.DataAnnotations;
using Playtestr.Api.Models;

namespace Playtestr.Api.Dtos;

  public record CreateQuestionRequest
  (
      [Required, MaxLength(255)] string Text,
      [EnumDataType(typeof(QuestionType))] QuestionType Type
  );
