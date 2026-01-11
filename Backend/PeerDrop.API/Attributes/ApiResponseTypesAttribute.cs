using Microsoft.AspNetCore.Mvc;
namespace PeerDrop.API.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ApiResponseTypesAttribute(Type successType, int successStatusCode = StatusCodes.Status200OK)
    : ProducesResponseTypeAttribute(successType, successStatusCode);

[AttributeUsage(AttributeTargets.All)]
public class StandardResponseTypesAttribute(Type dataType, int successStatusCode = StatusCodes.Status200OK)
    : Attribute
{
    public Type DataType { get; } = dataType;
    public int SuccessStatusCode { get; } = successStatusCode;

}
