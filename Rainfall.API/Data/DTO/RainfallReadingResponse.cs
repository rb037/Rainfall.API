using Newtonsoft.Json;

namespace Rainfall.API.Data.DTO
{
    /// <summary>
    /// Details of a rainfall reading response
    /// </summary>
    public class RainfallReadingResponse
    {
        /// <summary>
        /// The list of rainfall readings
        /// </summary>
        [JsonProperty("items")]
        public List<RainfallReading>? Readings { get; set; }
    }
}
