namespace Playtestr.Api.Services;

public enum OperationStatus { Success, NotFound, Forbidden, Conflict }

public record OperationResult<T>(OperationStatus Status, T? Value = default);