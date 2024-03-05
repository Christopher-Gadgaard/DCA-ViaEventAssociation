using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;

public class ViaGuestName:ValueObject
{
    public string Value { get; private init; }
    
    public ViaGuestName(string value)
    {
        Value = value;
      
    }
    public static OperationResult<ViaGuestName> Create(string guestName)
    {
        var validation = Validate(guestName);
        if (validation.IsSuccess)
        {
            return new ViaGuestName(guestName);
        }
        return validation.OperationErrors;
    }

    private static OperationResult<ViaGuestName> Validate(string guestName)
    {
        if (string.IsNullOrWhiteSpace(guestName))
        {
            return OperationResult<ViaGuestName>.Failure(new List<OperationError> { new OperationError(ErrorCode.InvalidInput, "Guest name cannot be null or empty.") });
        }
        if (guestName.Length < 3 || guestName.Length > 75)
        {
            return OperationResult<ViaGuestName>.Failure(new List<OperationError>{new OperationError(ErrorCode.InvalidInput, "Guest name must be between 3 and 75 characters.")});
        }
        return OperationResult<ViaGuestName>.Success(new ViaGuestName(guestName));
    }
  
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}