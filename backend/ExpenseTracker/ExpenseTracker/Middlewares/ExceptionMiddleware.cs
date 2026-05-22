using System.Net;
using System.Text.Json;

namespace ExpenseTracker.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
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
                _logger.LogError(ex, "Uma exceção não tratada ocorreu durante a requisição HTTP: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var statusCode = exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = statusCode == HttpStatusCode.InternalServerError 
                    ? "Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde."
                    : exception.Message
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            if (_env.IsDevelopment() && statusCode == HttpStatusCode.InternalServerError)
            {
                var devResponse = new
                {
                    response.StatusCode,
                    response.Message,
                    Details = exception.StackTrace
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(devResponse, options));
                return;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
