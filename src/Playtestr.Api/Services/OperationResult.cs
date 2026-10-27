namespace Playtestr.Api.Services;

public enum OperationStatus { Success, NotFound, Forbidden, Conflict, Invalid }

public record OperationResult<T>(OperationStatus Status, T? Value = default, string? Error = null);
