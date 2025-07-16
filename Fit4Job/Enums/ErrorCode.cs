namespace Fit4Job.Enums
{
    public enum ErrorCode
    {
        None = 0,                    // No error
        UnknownError = 1,            // General error fallback
        ValidationError = 100,       // Model validation failure
        NotFound = 101,              // Resource not found
        Unauthorized = 102,          // Authentication failed
        Forbidden = 103,             // Authorization denied
        Conflict = 104,              // Conflict with current state (e.g., duplicate)
        BadRequest = 105,            // Invalid input or malformed request
        InternalServerError = 106,   // Unexpected server-side exception
        DatabaseError = 107,         // DB-related failure
        Timeout = 108,               // Operation timed out
        ExternalServiceError = 109,  // API/third-party integration failed
        TokenExpired = 110,          // JWT or auth token expired
        TokenInvalid = 111,          // JWT or auth token invalid
        RateLimitExceeded = 112,     // Too many requests
        EmailAlreadyExists = 120,    // Used in registration
        UsernameAlreadyExists = 121, // Used in registration
        InvalidCredentials = 122,     // Login failure
        EmailNotConfirmed = 123
        
    }
}
