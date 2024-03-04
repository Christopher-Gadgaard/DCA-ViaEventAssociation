using Via.EventAssociation.Core.Domain.Common.Bases;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocationId :ValueObject
{
    public Guid Value { get; private init; }
    
    public ViaLocationId(Guid value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}