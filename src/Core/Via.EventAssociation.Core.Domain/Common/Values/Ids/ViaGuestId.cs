using Via.EventAssociation.Core.Domain.Common.Bases;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaGuestId :ValueObject
{
    public Guid Value { get; private init; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}