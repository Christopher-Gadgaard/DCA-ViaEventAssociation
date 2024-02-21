namespace ViaEventAssociation.Core.Tools.OperationResult.OperationError;

public abstract class OperationError
{
    public string Code { get; }
    public string Message { get; }
    public string? Type { get; }

    protected OperationError(string code, string message, string? type = null)
    {
        Code = code;
        Message = message;
        Type = type;
    }
}
