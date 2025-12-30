namespace PeerDrop.BLL.Exceptions;

/// <summary>
/// Exception for 401 Unauthorized - Authentication failures
/// </summary>
public class UnauthorizedException : Exception
{
    public string? ErrorCode { get; }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
