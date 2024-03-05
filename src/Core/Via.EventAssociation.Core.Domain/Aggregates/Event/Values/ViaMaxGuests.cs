using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event.Values;

public class ViaMaxGuests : ValueObject
{
    public int Value { get; private init; }

    private ViaMaxGuests(int value)
    {
        Value = value;
    }

    public static OperationResult<ViaMaxGuests> Create(int maxGuests)
    {
        var validation = Validate(maxGuests);
        if (validation.IsSuccess)
        {
            return new ViaMaxGuests(maxGuests);
        }

        return validation.OperationErrors;
    }

    private static OperationResult<int> Validate(int maxGuests)
    {
        if (maxGuests is < 5 or > 50)
        {
            return new OperationError(ErrorCode.InvalidInput, "Guest must be between 5 and 50 guests.");
        }

        return OperationResult<int>.Success(maxGuests);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}