using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event.Values;

public class ViaEventDescription : ValueObject
{
    public string Value { get; private init; }

    private ViaEventDescription(string value)
    {
        Value = value;
    }

    public static OperationResult<ViaEventDescription> Create(string description)
    {
        var validation = Validate(description);
        if (validation.IsSuccess)
        {
            return new ViaEventDescription(description);
        }

        return validation.OperationErrors;
    }

    private static OperationResult<string> Validate(string description)
    {
        if (description.Length > 250)
        {
            return new OperationError(ErrorCode.InvalidInput,
                $"Description cannot exceed 250 characters. {description.Length}/250");
        }

        return OperationResult<string>.SuccessWithPayload(description);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}