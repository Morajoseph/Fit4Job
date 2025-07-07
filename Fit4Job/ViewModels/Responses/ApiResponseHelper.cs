namespace Fit4Job.ViewModels.Responses
{
    public static class ApiResponseHelper
    {
        public static SuccessApiResponse<T> Success<T>(T data, string message = "Operation completed successfully")
        {
            return new SuccessApiResponse<T>()
            {
                Data = data,
                Success = true,
                Message = message,
                ErrorCode = ErrorCode.None
            };
        }

        public static ErrorApiResponse Error(ErrorCode errorCode, string message = "")
        {
            return new ErrorApiResponse()
            {
                Data = default,
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }
}
