using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rainfall.API.Data.Query;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Rainfall.API.Data.DTO;

namespace Rainfall.API.Controllers
{
    /// <summary>
    /// RainfallController
    /// </summary>
    [ApiController]
    [Route("rainfall")]
    [Produces("application/json")]
    [SwaggerTag("Operations relating to rainfall")]
    public class RainfallController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RainfallController> _logger;

        /// <summary>
        /// /// Operations relating to rainfall333
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public RainfallController(IMediator mediator, ILogger<RainfallController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get rainfall readings by station Id
        /// </summary>
        /// <remarks>
        /// Retrieve the latest readings for the specified stationId
        /// </remarks>
        [SwaggerResponse(200, "A list of rainfall readings successfully retrieved", typeof(RainfallReadingResponse))]
        [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse))]
        [SwaggerResponse(404, "No readings found for the specified stationId", typeof(ErrorResponse))]
        [SwaggerResponse(500, "Internal server error", typeof(ErrorResponse))]
        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(
            [FromRoute, SwaggerParameter("The id of the reading station", Required = true)] string stationId,
            [FromQuery, SwaggerParameter("The number of readings to return", Required = false), Range(1, 100), DefaultValue(10)] int count)
        {
            var query = new GetRainfallDataQuery { StationId = stationId, Count = count };
            var readings = await _mediator.Send(query);
            if (readings == null)
            {
                var errorResponse = new ErrorResponse { Message = "No readings found for the specified stationId" };
                return NotFound(errorResponse);
            }
            return Ok(readings);
        }
    }
}
