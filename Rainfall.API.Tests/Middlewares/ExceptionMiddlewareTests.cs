using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Rainfall.API.Middlewares;

namespace Rainfall.API.Tests.Middlewares
{
    [TestFixture]
    public class ExceptionMiddlewareTests
    {
        private Mock<ILogger<ExceptionMiddleware>> _loggerMock;
        private ExceptionMiddleware _middleware;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ExceptionMiddleware>>();
            RequestDelegate next = (HttpContext hc) => Task.CompletedTask;
            _middleware = new ExceptionMiddleware(next, _loggerMock.Object);
        }

        [Test]
        public async Task InvokeAsync_CallsNextDelegate_WhenNoExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Never);
        }

        [Test]
        public async Task InvokeAsync_CatchesException_WhenExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            RequestDelegate next = (HttpContext hc) => throw new HttpRequestException();
            _middleware = new ExceptionMiddleware(next, _loggerMock.Object);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
        }
    }

}
