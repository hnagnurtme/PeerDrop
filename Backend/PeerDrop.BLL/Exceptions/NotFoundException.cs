namespace PeerDrop.BLL.Exceptions;

public class NotFoundException : Exception
{
    public string? ErrorCode { get; }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public NotFoundException(string entityName, object key) 
        : base($"{entityName} with key '{key}' was not found.")
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
