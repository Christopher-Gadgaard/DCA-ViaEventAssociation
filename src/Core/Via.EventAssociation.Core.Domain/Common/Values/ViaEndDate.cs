using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values;

public class ViaEndDate : ValueObject
{
    public DateTime Value { get; private init; }

    private ViaEndDate(DateTime value)
    {
        Value = value;
    }

    public static OperationResult<ViaEndDate> Create(DateTime endDate)
    {
        return new ViaEndDate(endDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}