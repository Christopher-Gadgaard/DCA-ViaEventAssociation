namespace ViaEventAssociation.Core.Tools.OperationResult.OperationError;
public class OperationError
{
    public ErrorCode Code { get; }
    public string Message => Code.GetMessage();
    public string? Type { get; } 

    public OperationError(ErrorCode code, string? type = null)
    {
        Code = code;
        Type = type;
    }
}

