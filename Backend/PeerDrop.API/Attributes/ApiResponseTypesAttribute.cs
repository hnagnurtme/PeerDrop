using Microsoft.AspNetCore.Mvc;
namespace PeerDrop.API.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ApiResponseTypesAttribute : ProducesResponseTypeAttribute
{
    public ApiResponseTypesAttribute(Type successType, int successStatusCode = StatusCodes.Status200OK)
        : base(successType, successStatusCode)
    {
    
    }
}
public class StandardResponseTypesAttribute : Attribute
{
    public Type DataType { get; }
    public int SuccessStatusCode { get; }

    public StandardResponseTypesAttribute(Type dataType, int successStatusCode = StatusCodes.Status200OK)
    {
        DataType = dataType;
        SuccessStatusCode = successStatusCode;
    }
}
