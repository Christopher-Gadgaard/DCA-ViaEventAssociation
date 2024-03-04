using Via.EventAssociation.Core.Domain.Common.Bases;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaBookingId :ValueObject
{
    public Guid Value { get; private init; }
    
    public ViaBookingId(Guid value)
    {
        Value = value;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}