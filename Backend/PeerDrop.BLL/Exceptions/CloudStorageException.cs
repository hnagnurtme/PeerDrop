namespace PeerDrop.BLL.Exceptions;

public class CloudStorageException : Exception
{
    public string? ErrorCode { get; }

    public CloudStorageException(string message) : base(message)
    {
    }

    public CloudStorageException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    public CloudStorageException(string entityName, object key) 
        : base($"{entityName} with key '{key}' was not found.")
    {
    }

    public CloudStorageException(string message, Exception innerException) : base(message, innerException)
    {
    }
}