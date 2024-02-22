namespace ViaEventAssociation.Core.Tools.OperationResult.OperationError
{
    public class OperationError
    {
        public ErrorCode Code { get; }
        public string Message { get; private set; }
        public string? Type { get; }

        public OperationError(ErrorCode code, string specificMessage = null, string? type = null)
        {
            Code = code;
            Type = type;
            Message = specificMessage ?? code.GetDescription();
        }
    }
}