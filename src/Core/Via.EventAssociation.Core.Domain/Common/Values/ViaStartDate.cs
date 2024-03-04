using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
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
        // Check that the event does not start before 08:00 AM
        if (startDate.TimeOfDay < new TimeSpan(8, 0, 0))
        {
            return new OperationError(ErrorCode.InvalidInput, "Event cannot start before 08:00 AM.");
        }

        return new ViaStartDate(startDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}