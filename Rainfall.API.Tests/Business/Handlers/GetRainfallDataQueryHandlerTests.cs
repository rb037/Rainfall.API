using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Rainfall.API.Business.Handlers;
using Rainfall.API.Data.DTO;
using Rainfall.API.Data.Query;
using System.Net;
using System.Text;

namespace Rainfall.API.Tests.Business.Handlers
{
    [TestFixture]
    public class GetRainfallDataQueryHandlerTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<ILogger<GetRainfallDataQueryHandler>> _loggerMock;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private GetRainfallDataQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com") // mock base address
            };
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _loggerMock = new Mock<ILogger<GetRainfallDataQueryHandler>>();

            _handler = new GetRainfallDataQueryHandler(_httpClientFactoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsRainfallReadingResponse_WhenResponseIsSuccessful()
        {
            // Arrange
            var rainfallReadingResponse = new RainfallReadingResponse
            {
                Readings = new List<RainfallReading>() {
                new() { DateMeasured = DateTime.UtcNow, AmountMeasured = 1 } }
            };
            var json = JsonConvert.SerializeObject(rainfallReadingResponse);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = httpContent };
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var query = new GetRainfallDataQuery {  };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Readings!, Has.Count.EqualTo(rainfallReadingResponse.Readings.Count));
            Assert.Multiple(() =>
            {
                Assert.That(result.Readings![0].AmountMeasured, Is.EqualTo(rainfallReadingResponse.Readings[0].AmountMeasured));
                Assert.That(result.Readings[0].DateMeasured, Is.EqualTo(rainfallReadingResponse.Readings[0].DateMeasured));
            });
        }

        [Test]
        public void Handle_ThrowsHttpRequestException_WhenResponseIsUnsuccessful()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var query = new GetRainfallDataQuery {  };

            // Act & Assert
            Assert.ThrowsAsync<HttpRequestException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }

}
