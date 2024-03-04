using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocationName:ValueObject
{
    public string Value { get; private init; }

    public ViaLocationName(string value)
    {
        Value = value;
        Validate();
    }

    private OperationResult<string> Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return new OperationError(ErrorCode.InvalidInput, "Location name cannot be null or empty.");
        }

        if (Value.Length < 3 || Value.Length > 75)
        {
            return new OperationError(ErrorCode.InvalidInput, "Location name must be between 3 and 75 characters.");
        }

        return Value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}