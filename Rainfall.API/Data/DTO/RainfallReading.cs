using Newtonsoft.Json;

namespace Rainfall.API.Data.DTO
{
    /// <summary>
    /// Details of a rainfall reading
    /// </summary>
    public class RainfallReading
    {
        /// <summary>
        /// The date and time the reading was taken
        /// </summary>
        [JsonProperty("dateTime")]
        public DateTime DateMeasured { get; set; }

        /// <summary>
        /// The amount of rainfall measured
        /// </summary>
        [JsonProperty("value")]
        public decimal AmountMeasured { get; set; }
    }
}
