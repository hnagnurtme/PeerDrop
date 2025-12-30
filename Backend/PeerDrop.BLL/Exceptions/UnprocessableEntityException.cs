namespace PeerDrop.BLL.Exceptions;

/// <summary>
/// Exception for 422 Unprocessable Entity - Validation/Business rule violations
/// </summary>
public class UnprocessableEntityException : Exception
{
    public string? ErrorCode { get; }

    public UnprocessableEntityException(string message) : base(message)
    {
    }

    public UnprocessableEntityException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public UnprocessableEntityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
