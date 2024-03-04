using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
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
        // Validate that the event does not end after 22:00 (10 PM)
        if (endDate.TimeOfDay > new TimeSpan(22, 0, 0))
        {
            return new OperationError(ErrorCode.InvalidInput, "Event cannot end after 22:00.");
        }

        return new ViaEndDate(endDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}