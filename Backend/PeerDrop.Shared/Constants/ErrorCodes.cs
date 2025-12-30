namespace PeerDrop.Shared.Constants;

public static class ErrorCodes
{
    // Authentication (AUTH)
    public const string AuthInvalidCredentials = "AUTH_INVALID_CREDENTIALS";
    public const string AuthAccountDisabled = "AUTH_ACCOUNT_DISABLED";
    public const string AuthEmailAlreadyExists = "AUTH_EMAIL_ALREADY_EXISTS";
    public const string AuthInvalidToken = "AUTH_INVALID_TOKEN";
    public const string AuthTokenExpired = "AUTH_TOKEN_EXPIRED";
    public const string AuthUnauthorized = "AUTH_UNAUTHORIZED";
    public const string AuthForbidden = "AUTH_FORBIDDEN";

    // User (USER)
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";

    // Validation (VALIDATION)
    public const string ValidationFailed = "VALIDATION_FAILED";
    public const string ValidationRequiredField = "VALIDATION_REQUIRED_FIELD";
    public const string ValidationInvalidEmail = "VALIDATION_INVALID_EMAIL";
    public const string ValidationPasswordTooShort = "VALIDATION_PASSWORD_TOO_SHORT";
    public const string ValidationPasswordMismatch = "VALIDATION_PASSWORD_MISMATCH";

    // General (GEN)
    public const string InternalServerError = "GEN_INTERNAL_SERVER_ERROR";
    public const string NotFound = "GEN_NOT_FOUND";
    public const string BadRequest = "GEN_BAD_REQUEST";
}