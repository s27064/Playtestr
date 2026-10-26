namespace Playtestr.Api.Services;

public enum OperationStatus { Success, NotFound, Forbidden }

public record OperationResult<T>(OperationStatus Status, T? Value = default);