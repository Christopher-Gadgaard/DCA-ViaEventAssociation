namespace ViaEventAssociation.Core.Tools.OperationResult.OperationError;

public enum ErrorCode
{
    InvalidInput,
    ProcessingFailed,
    NotFound,
    UnauthorizedAccess,
    Forbidden,
    Conflict,
    BadRequest,
    InternalServerError,
    ServiceUnavailable,
    Timeout
}
public static class ErrorCodeExtensions
{
    public static string GetMessage(this ErrorCode errorCode)
    {
        return errorCode switch
        {
            ErrorCode.InvalidInput => "Input data cannot be null or empty.",
            ErrorCode.ProcessingFailed => "Data processing failed due to an internal error.",
            ErrorCode.NotFound => "The requested resource was not found.",
            ErrorCode.UnauthorizedAccess => "Access denied due to invalid credentials.",
            ErrorCode.Forbidden => "You do not have permission to access the requested resource.",
            ErrorCode.Conflict => "The request could not be completed due to a conflict with the current state of the resource.",
            ErrorCode.BadRequest => "The request could not be understood by the server due to malformed syntax.",
            ErrorCode.InternalServerError => "The server encountered an unexpected condition which prevented it from fulfilling the request.",
            ErrorCode.ServiceUnavailable => "The server is currently unable to handle the request due to a temporary overloading or maintenance.",
            ErrorCode.Timeout => "The request timed out due to server taking too long to respond.",
            _ => "An unknown error occurred."
        };
    }
}
