using System.Net;
using System.Text.Json;
using ExpenseTracker.Middlewares;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExpenseTracker.Tests.Middlewares
{
    public class ExceptionMiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionMiddleware>> _loggerMock;
        private readonly Mock<IHostEnvironment> _envMock;

        public ExceptionMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
            _envMock = new Mock<IHostEnvironment>();
        }

        [Fact]
        public async Task InvokeAsync_WhenNoExceptionIsThrown_ShouldCallNextDelegate()
        {
            var context = new DefaultHttpContext();
            bool nextCalled = false;
            RequestDelegate next = (ctx) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, _envMock.Object);

            await middleware.InvokeAsync(context);

            nextCalled.Should().BeTrue();
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task InvokeAsync_WhenArgumentExceptionIsThrown_ShouldReturnBadRequest()
        {
            RequestDelegate next = (ctx) => throw new ArgumentException("Dados de entrada inválidos.");
            var context = new DefaultHttpContext();

            var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            _envMock.Setup(e => e.EnvironmentName).Returns("Production");

            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, _envMock.Object);

            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            context.Response.ContentType.Should().Be("application/json");

            memoryStream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(memoryStream);
            var responseBody = await reader.ReadToEndAsync();

            var jsonDocument = JsonDocument.Parse(responseBody);
            jsonDocument.RootElement.GetProperty("statusCode").GetInt32().Should().Be(400);
            jsonDocument.RootElement.GetProperty("message").GetString().Should().Be("Dados de entrada inválidos.");
        }

        [Fact]
        public async Task InvokeAsync_WhenGenericExceptionIsThrownInProduction_ShouldReturnInternalServerErrorWithoutDetails()
        {
            RequestDelegate next = (ctx) => throw new Exception("Erro crítico de banco de dados.");
            var context = new DefaultHttpContext();

            var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            _envMock.Setup(e => e.EnvironmentName).Returns("Production");

            var middleware = new ExceptionMiddleware(next, _loggerMock.Object, _envMock.Object);

            await middleware.InvokeAsync(context);

            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            memoryStream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(memoryStream);
            var responseBody = await reader.ReadToEndAsync();

            var jsonDocument = JsonDocument.Parse(responseBody);
            jsonDocument.RootElement.GetProperty("message").GetString().Should().Be("Ocorreu um erro interno no servidor. Por favor, tente novamente mais tarde.");
            jsonDocument.RootElement.TryGetProperty("details", out _).Should().BeFalse();
        }
    }
}
