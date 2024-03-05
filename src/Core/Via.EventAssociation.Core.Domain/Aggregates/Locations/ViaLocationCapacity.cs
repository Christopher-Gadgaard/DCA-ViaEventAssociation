using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocationCapacity :ValueObject
{
    public int Value { get; private init; }
    
    public ViaLocationCapacity(int value)
    {
        Value = value;
        Validate(value);
    }
    
    public static OperationResult<ViaLocationCapacity> Create(int capacity)
    {
        var validation = Validate(capacity);
        if (validation.IsSuccess)
        {
            return new ViaLocationCapacity(capacity);
        }
        return validation.OperationErrors;
    }
    public static OperationResult<int> Validate(int capacity)
    {
        if (capacity <=0)
        {
            return new OperationError(ErrorCode.InvalidInput, "Location capacity cannot be negative or zero.");
        }
        if(capacity > 100)
        {
            return new OperationError(ErrorCode.InvalidInput, "Location capacity cannot be greater than 100.");
        }
        return OperationResult<int>.Success(capacity);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}