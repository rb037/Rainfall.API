namespace Rainfall.API.Data.DTO
{
    /// <summary>
    /// Details of an invalid request property
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// The name of the invalid property
        /// </summary>
        public string? PropertyName { get; set; }

        /// <summary>
        /// The error message
        /// </summary>
        public string? Message { get; set; }
    }
}
