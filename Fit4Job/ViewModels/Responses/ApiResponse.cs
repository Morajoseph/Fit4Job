namespace Fit4Job.ViewModels.Responses
{
    // Base Response Models
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public DateTime Timestamp { get; set; }
        public string RequestId { get; set; }

        public ApiResponse()
        {
            Success = true;
            Errors = new List<string>();
            Timestamp = DateTime.UtcNow;
            RequestId = Guid.NewGuid().ToString();
        }

        public ApiResponse(T data, string message = null) : this()
        {
            Data = data;
            Message = message ?? "Operation completed successfully";
        }

        public ApiResponse(string error) : this()
        {
            Success = false;
            Errors.Add(error);
            Message = "Operation failed";
        }

        public ApiResponse(List<string> errors) : this()
        {
            Success = false;
            Errors = errors ?? new List<string>();
            Message = "Operation failed";
        }
    }
}
