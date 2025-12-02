using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Fcg.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO CAPTURADO: {ex.GetType().Name} - {ex.Message}");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        object errorResponse;

        switch (exception)
        {
            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new
                {
                    success = false,
                    message = "Recurso não encontrado.",
                    error = exception.Message,
                    timestamp = DateTime.UtcNow
                };
                break;

            case ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new
                {
                    success = false,
                    message = exception.Message,
                    error = "Dados inválidos",
                    timestamp = DateTime.UtcNow
                };
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse = new
                {
                    success = false,
                    message = "Acesso não autorizado.",
                    error = exception.Message,
                    timestamp = DateTime.UtcNow
                };
                break;

            case InvalidOperationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new
                {
                    success = false,
                    message = "Operação inválida.",
                    error = exception.Message,
                    timestamp = DateTime.UtcNow
                };
                break;

            
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                
                var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

                errorResponse = new
                {
                    success = false,
                    message = "Erro interno no servidor.",
                    error = isDevelopment ? exception.Message : null,
                    stackTrace = isDevelopment ? exception.StackTrace : null,
                    timestamp = DateTime.UtcNow
                };
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}