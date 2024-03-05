using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;

public class ViaGuestName:ValueObject
{
    private ViaName _name { get; }
    private ViaName _lastName { get; }

    private ViaGuestName(ViaName guestFirstName, ViaName guestLastName)
    {
        _name = guestFirstName;
        _lastName = guestLastName;
    }

    public static OperationResult<ViaGuestName> Create(string guestFirstName, string guestLastName)
    {
        var firstNameResult = ViaName.Create(guestFirstName);
        var lastNameResult = ViaName.Create(guestLastName);

        if (!firstNameResult.IsSuccess)
        {
            return OperationResult<ViaGuestName>.Failure(firstNameResult.OperationErrors);
        }

        if (!lastNameResult.IsSuccess)
        {
            return OperationResult<ViaGuestName>.Failure(lastNameResult.OperationErrors);
        }

        return OperationResult<ViaGuestName>.SuccessWithPayload(new ViaGuestName(firstNameResult.Payload, lastNameResult.Payload));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _name;
        yield return _lastName;
    }
}