using MediatR;
using Newtonsoft.Json;
using Rainfall.API.Data.DTO;
using Rainfall.API.Data.Query;

namespace Rainfall.API.Business.Handlers
{
    /// <summary>
    /// Handler for getting rainfall data.
    /// </summary>
    public class GetRainfallDataQueryHandler : IRequestHandler<GetRainfallDataQuery, RainfallReadingResponse>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GetRainfallDataQueryHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRainfallDataQueryHandler"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="logger"></param>
        public GetRainfallDataQueryHandler(IHttpClientFactory httpClientFactory, ILogger<GetRainfallDataQueryHandler> logger)
        {
            _httpClient = httpClientFactory.CreateClient(nameof(GetRainfallDataQueryHandler));
            _logger = logger;
        }

        /// <summary>
        /// Handles the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<RainfallReadingResponse> Handle(GetRainfallDataQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting rainfall data for station {StationId} with count {Count}", request.StationId, request.Count);
            
            var response = await _httpClient.GetAsync($"id/stations/{request.StationId}/readings?_limit={request.Count}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get rainfall data for station {StationId}. Status code: {StatusCode}", request.StationId, response.StatusCode);
                throw new HttpRequestException($"Failed to get rainfall data for station {request.StationId}", null, response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<RainfallReadingResponse>(content)!;

            _logger.LogInformation("Successfully got {Count} rainfall readings for station {StationId}", result.Readings?.Count, request.StationId);
            return result;
        }
    }
}
