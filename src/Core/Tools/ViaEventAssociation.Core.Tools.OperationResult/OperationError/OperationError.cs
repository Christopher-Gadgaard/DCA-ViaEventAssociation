namespace ViaEventAssociation.Core.Tools.OperationResult.OperationError
{
    public class OperationError(ErrorCode code, string? specificMessage = null, string? type = null)
    {
        public ErrorCode Code { get; } = code;
        public string? Message { get; private set; } = specificMessage ?? code.GetDescription();
        public string? Type { get; } = type;
    }
}
