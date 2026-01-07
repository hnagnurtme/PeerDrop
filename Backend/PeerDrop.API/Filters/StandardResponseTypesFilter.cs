using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PeerDrop.API.Attributes;
using PeerDrop.Shared.Responses;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PeerDrop.API.Filters;

/// <summary>
/// Automatically adds standard response types to endpoints based on conventions
/// </summary>
public class StandardResponseTypesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var standardAttr = context.MethodInfo.GetCustomAttributes(true)
            .OfType<StandardResponseTypesAttribute>()
            .FirstOrDefault();

        if (standardAttr == null) return;

        // Clear existing responses to rebuild them
        var existingResponses = operation.Responses.ToDictionary(r => r.Key, r => r.Value);
        operation.Responses.Clear();

        var dataType = standardAttr.DataType;
        var successCode = standardAttr.SuccessStatusCode.ToString();

        // Success response with specific data type
        var successResponseType = typeof(ApiResponse<>).MakeGenericType(dataType);
        AddResponse(operation, context, successCode, "Success", successResponseType);

        // Common error responses
        var errorType = typeof(ApiResponse<object>);
        
        AddResponse(operation, context, "400", "Bad Request - Invalid input data", errorType);

        var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true ||
                          context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
        var hasAllowAnonymous = context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

        if (hasAuthorize && !hasAllowAnonymous)
        {
            AddResponse(operation, context, "401", "Unauthorized - Authentication required", errorType);
            AddResponse(operation, context, "403", "Forbidden - Insufficient permissions", errorType);
        }

        var httpMethod = context.ApiDescription.HttpMethod?.ToUpper();
        var hasRouteParams = context.ApiDescription.ParameterDescriptions.Any(p => p.Source.Id == "Path");

        if (hasRouteParams && (httpMethod == "GET" || httpMethod == "PUT" || httpMethod == "DELETE"))
        {
            AddResponse(operation, context, "404", "Not Found - Resource does not exist", errorType);
        }

        if (httpMethod == "POST" || httpMethod == "PUT")
        {
            AddResponse(operation, context, "409", "Conflict - Resource already exists or version mismatch", errorType);
            AddResponse(operation, context, "422", "Unprocessable Entity - Validation failed", errorType);
        }

        AddResponse(operation, context, "500", "Internal Server Error - An unexpected error occurred", errorType);
    }

    private void AddResponse(OpenApiOperation operation, OperationFilterContext context, string statusCode, string description, Type responseType)
    {
        var schema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository);
        
        operation.Responses[statusCode] = new OpenApiResponse
        {
            Description = description,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = schema
                }
            }
        };
    }
}
