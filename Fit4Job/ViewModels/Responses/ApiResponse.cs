namespace Fit4Job.ViewModels.Responses
{
    // Base Response Models
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; } = true;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public ErrorCode? ErrorCode { get; set; } = Enums.ErrorCode.None;
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public ApiResponse() { }
        public ApiResponse(T data, string massage , bool success , ErrorCode errorCode)
        {
            Data = data;
            Message = massage;
            Success = success;
            ErrorCode = errorCode;
        }
    }
}
