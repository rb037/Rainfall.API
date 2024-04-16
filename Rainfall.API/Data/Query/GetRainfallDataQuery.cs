using MediatR;
using Rainfall.API.Data.DTO;

namespace Rainfall.API.Data.Query
{
    /// <summary>
    /// GetRainfallDataQuery
    /// </summary>
    public class GetRainfallDataQuery : IRequest<RainfallReadingResponse>
    {
        /// <summary>
        /// StationId
        /// </summary>
        public string? StationId { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; set; }
    }

}
