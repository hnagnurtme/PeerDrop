namespace PeerDrop.BLL.Exceptions;

public class FileTooLargeException : Exception
{
    public string? ErrorCode { get; }

    public FileTooLargeException(string message) : base(message)
    {
    }

    public FileTooLargeException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public FileTooLargeException(string entityName, object key) 
        : base($"{entityName} with key '{key}' was not found.")
    {
    }

    public FileTooLargeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}