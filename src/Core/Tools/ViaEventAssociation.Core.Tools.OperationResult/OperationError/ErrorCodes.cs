using System.ComponentModel;
using System.Reflection;
namespace ViaEventAssociation.Core.Tools.OperationResult.OperationError;

public enum ErrorCode
{
    [Description("Input data cannot be null or empty.")]
    InvalidInput,
    [Description("Data processing failed due to an internal error.")]
    ProcessingFailed,
    [Description("The requested resource was not found.")]
    NotFound,
    [Description("Access denied due to invalid credentials.")]
    UnauthorizedAccess,
    [Description("You do not have permission to access the requested resource.")]
    Forbidden,
    [Description("The request could not be completed due to a conflict with the current state of the resource.")]
    Conflict,
    [Description("The request could not be understood by the server due to malformed syntax.")]
    BadRequest,
    [Description("The server encountered an unexpected condition which prevented it from fulfilling the request.")]
    InternalServerError,
    [Description("The server is currently unable to handle the request due to a temporary overloading or maintenance.")]
    ServiceUnavailable,
    [Description("The request timed out due to server taking too long to respond.")]
    Timeout
}

public static class ErrorCodeExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo? fi = value.GetType().GetField(value.ToString());

        if (fi is null) return value.ToString(); 

        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes.Length > 0)
        {
            return attributes[0].Description;
        }
        
        return value.ToString();
    }

}

