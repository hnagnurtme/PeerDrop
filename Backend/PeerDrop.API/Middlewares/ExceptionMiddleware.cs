using System.Net;
using System.Text.Json;
using PeerDrop.BLL.Exceptions;
using PeerDrop.Shared.Constants;
using PeerDrop.Shared.Responses;

namespace PeerDrop.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionMiddleware> logger)
    {
        context.Response.ContentType = "application/json";

        // Log based on exception severity
        var (statusCode, response, logLevel) = exception switch
        {
            // 401 Unauthorized - Log as Warning (expected authentication failures)
            UnauthorizedException unauthorizedEx => (
                (int)HttpStatusCode.Unauthorized,
                ApiResponse<object>.Fail(
                    unauthorizedEx.Message,
                    errorCode: unauthorizedEx.ErrorCode ?? ErrorCodes.AuthUnauthorized),
                LogLevel.Warning
            ),
            UnauthorizedAccessException => (
                (int)HttpStatusCode.Unauthorized,
                ApiResponse<object>.Fail(
                    "Unauthorized access",
                    errorCode: ErrorCodes.AuthUnauthorized),
                LogLevel.Warning
            ),
            
            // 403 Forbidden - Log as Warning (expected authorization failures)
            ForbiddenException forbiddenEx => (
                (int)HttpStatusCode.Forbidden,
                ApiResponse<object>.Fail(
                    forbiddenEx.Message,
                    errorCode: forbiddenEx.ErrorCode ?? ErrorCodes.AuthForbidden),
                LogLevel.Warning
            ),
            
            // 404 Not Found - Log as Information (common, expected scenario)
            NotFoundException notFoundEx => (
                (int)HttpStatusCode.NotFound,
                ApiResponse<object>.Fail(
                    notFoundEx.Message,
                    errorCode: notFoundEx.ErrorCode ?? ErrorCodes.NotFound),
                LogLevel.Information
            ),
            
            // 422 Unprocessable Entity - Log as Warning (validation/business rule violations)
            UnprocessableEntityException unprocessableEx => (
                (int)HttpStatusCode.UnprocessableEntity,
                ApiResponse<object>.Fail(
                    unprocessableEx.Message,
                    errorCode: unprocessableEx.ErrorCode ?? ErrorCodes.ValidationFailed),
                LogLevel.Warning
            ),
            
            // 400 Bad Request - Log as Warning (business exceptions)
            BusinessException businessEx => (
                (int)HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail(
                    businessEx.Message,
                    errorCode: businessEx.ErrorCode ?? ErrorCodes.BadRequest),
                LogLevel.Warning
            ),
            
            // 500 Internal Server Error - Log as Error (unexpected exceptions)
            _ => (
                (int)HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail(
                    "An internal server error occurred",
                    errorCode: ErrorCodes.InternalServerError),
                LogLevel.Error
            )
        };

        // Log with appropriate level
        logger.Log(logLevel, exception, 
            "HTTP {StatusCode}: {ExceptionType} - {Message}", 
            statusCode, 
            exception.GetType().Name, 
            exception.Message);

        context.Response.StatusCode = statusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
