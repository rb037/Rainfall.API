using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Rainfall.API.Controllers;
using Rainfall.API.Data.DTO;
using Rainfall.API.Data.Query;

namespace Rainfall.API.Tests
{
    public class RainfallControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<RainfallController>> _loggerMock;
        private RainfallController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<RainfallController>>();
            _controller = new RainfallController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetRainfallReadings_ReturnsExpectedReadings_WhenReadingsExist()
        {
            // Arrange
            var stationId = "testStation";
            var count = 10;
            var expectedReadings = new RainfallReadingResponse { Readings = new List<RainfallReading>() { 
                new() { DateMeasured = DateTime.UtcNow, AmountMeasured = 1 } } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetRainfallDataQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(expectedReadings));

            // Act
            var result = await _controller.GetRainfallReadings(stationId, count);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var actualReadings = okResult.Value as RainfallReadingResponse;
            Assert.That(actualReadings, Is.EqualTo(expectedReadings));
        }

        [Test]
        public async Task GetRainfallReadings_ReturnsNotFoundResult_WhenNoReadingsExist()
        {
            // Arrange
            var stationId = "testStation";
            var count = 10;
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetRainfallDataQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult<RainfallReadingResponse>(null));

            // Act
            var result = await _controller.GetRainfallReadings(stationId, count);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }
    }
}
