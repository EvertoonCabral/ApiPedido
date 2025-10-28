using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


namespace Api.Pedidos.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest &&
                context.Items.TryGetValue("ModelStateErrors", out var modelStateObj) &&
                modelStateObj is SerializableError modelErrors)
            {
                await WriteJson(context, HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = "Erro de validação.",
                    errors = modelErrors,
                    traceId = context.TraceIdentifier
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception - TraceId: {TraceId}", context.TraceIdentifier);
            await WriteJson(context, HttpStatusCode.InternalServerError, new
            {
                success = false,
                message = "Erro interno. Tente novamente mais tarde.",
                traceId = context.TraceIdentifier
            });
        }
    }

    private static async Task WriteJson(HttpContext context, HttpStatusCode statusCode, object payload)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";
        }

        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });

        await context.Response.WriteAsync(json);
    }
}