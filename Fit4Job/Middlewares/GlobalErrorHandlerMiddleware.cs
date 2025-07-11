namespace Fit4Job.Middlewares
{
    public class GlobalErrorHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // Error log here !!

                ///
                var response = ApiResponseHelper.Error<bool>(ErrorCode.UnknownError , ex.Message);
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
