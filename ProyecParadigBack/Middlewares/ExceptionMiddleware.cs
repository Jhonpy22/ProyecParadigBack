using Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;      // Para IWebHostEnvironment
using Microsoft.Extensions.Hosting;

namespace ProyecParadigBack.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)      // <-- añadimos env
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception at {Path}", context.Request.Path);

                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    BusinessException => StatusCodes.Status409Conflict,
                    NotFoundException => StatusCodes.Status404NotFound,
                    ValidationException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };
                context.Response.StatusCode = statusCode;

                // Construimos un diccionario para manejar dinámicamente el stackTrace
                var errorResponse = new Dictionary<string, object>
                {
                    ["statusCode"] = statusCode,
                    ["error"] = GetErrorName(statusCode),
                    ["message"] = ex.Message,
                    ["timestamp"] = DateTime.UtcNow,
                    ["path"] = context.Request.Path
                };

                // Solo en Development incluimos la traza completa
                if (_env.IsDevelopment() && ex.StackTrace != null)
                {
                    errorResponse["stackTrace"] = ex.StackTrace;
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var jsonResponse = JsonSerializer.Serialize(errorResponse, options);

                await context.Response.WriteAsync(jsonResponse);
            }
        }

        private static string GetErrorName(int statusCode) =>
            statusCode switch
            {
                400 => "Bad Request",
                404 => "Not Found",
                409 => "Conflict",
                500 => "Internal Server Error",
                _ => "Error"
            };
    }
}
