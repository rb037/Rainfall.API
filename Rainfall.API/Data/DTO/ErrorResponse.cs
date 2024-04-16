namespace Rainfall.API.Data.DTO
{
    /// <summary>
    /// Details of an error response
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// The error message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// The details of the error
        /// </summary>
        public List<ErrorDetail>? Detail { get; set; }
    }
}
