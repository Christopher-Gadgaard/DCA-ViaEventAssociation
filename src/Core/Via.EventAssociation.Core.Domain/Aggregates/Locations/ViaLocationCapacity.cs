using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocationCapacity
{
    public int Value { get; private init; }
    
    public ViaLocationCapacity(int value)
    {
        Value = value;
        Validate();
    }
    public OperationResult<int> Validate()
    {
        if (Value < 0)
        {
            return new OperationError(ErrorCode.InvalidInput, "Location capacity cannot be negative.");
        }
        if(Value > 100)
        {
            return new OperationError(ErrorCode.InvalidInput, "Location capacity cannot be greater than 100.");
        }
        return Value;
    }
    
    
}