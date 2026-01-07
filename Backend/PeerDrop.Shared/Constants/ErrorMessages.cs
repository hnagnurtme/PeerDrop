namespace PeerDrop.Shared.Constants;

public static class ErrorMessages
{
    // Authentication
    public const string InvalidCredentials = "Invalid email or password.";
    public const string AccountDisabled = "Your account has been disabled.";
    public const string EmailAlreadyExists = "An account with this email already exists.";
    public const string InvalidToken = "Invalid or expired token.";
    public const string TokenExpired = "Token has expired.";
    public const string UnauthorizedAccess = "You are not authorized to perform this action.";

    // User
    public const string UserNotFound = "User not found.";
    public const string UserAlreadyExists = "User already exists.";

    // Validation
    public const string ValidationFailed = "One or more validation errors occurred.";
    public const string RequiredField = "This field is required.";
    public const string InvalidEmail = "Invalid email format.";
    public const string PasswordTooShort = "Password must be at least 6 characters.";
    public const string PasswordMismatch = "Passwords do not match.";

    // General
    public const string InternalServerError = "An internal server error occurred.";
    public const string NotFound = "The requested resource was not found.";
    public const string BadRequest = "Invalid request.";
    
    // File
    public const string FileTooLarge = "File is too large.";
    public const string CloudStorageUnauthorized = "Unauthorized access to cloud storage.";
    public const  string CloudStorageForbidden = "Cloud storage forbidden.";
    public const string CloudUploadFailed = "Cloud upload failed.";
    public const string InvalidFile = "Invalid file.";
}
