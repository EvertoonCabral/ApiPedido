namespace Api.Pedidos.Application.Common.Responses;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }
    public string? TraceId { get; init; }

    public static ApiResponse<T> Ok(T data, string message = "Operação realizada com sucesso.", string? traceId = null)
        => new() { Success = true, Message = message, Data = data, TraceId = traceId };

    public static ApiResponse<T> Fail(string message, string? traceId = null, T? data = default)
        => new() { Success = false, Message = message, Data = data, TraceId = traceId };
}