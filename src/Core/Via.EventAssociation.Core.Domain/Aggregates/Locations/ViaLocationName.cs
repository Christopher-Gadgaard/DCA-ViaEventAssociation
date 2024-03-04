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
        Validate(value);
    }

    public static OperationResult<ViaLocationName> Create(string locationName)
    {
        var validation = Validate(locationName);
        if (validation.IsSuccess)
        {
            return new ViaLocationName(locationName);
        }

        return validation.OperationErrors;
    }
    private static OperationResult<string> Validate( string locationName)
    {
        if (string.IsNullOrWhiteSpace(locationName))
        {
            return new OperationError(ErrorCode.InvalidInput, "Location name cannot be null or empty.");
        }

        if (locationName.Length < 3 || locationName.Length > 75)
        {
            return new OperationError(ErrorCode.InvalidInput, "Location name must be between 3 and 75 characters.");
        }

        return OperationResult<string>.SuccessWithPayload((locationName));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}