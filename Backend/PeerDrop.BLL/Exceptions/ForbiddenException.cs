namespace PeerDrop.BLL.Exceptions;

/// <summary>
/// Exception for 403 Forbidden - User is authenticated but lacks permission
/// </summary>
public class ForbiddenException : Exception
{
    public string? ErrorCode { get; }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
