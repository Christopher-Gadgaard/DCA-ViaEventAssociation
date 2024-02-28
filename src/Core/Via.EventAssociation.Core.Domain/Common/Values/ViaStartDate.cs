using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values;

public class ViaStartDate : ValueObject
{
    public DateTime Value { get; private init; }

    private ViaStartDate(DateTime value)
    {
        Value = value;
    }

    public static OperationResult<ViaStartDate> Create(DateTime startDate)
    {
        return new ViaStartDate(startDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}