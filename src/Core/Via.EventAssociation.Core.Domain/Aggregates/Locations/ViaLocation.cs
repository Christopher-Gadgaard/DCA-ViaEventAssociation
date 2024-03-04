using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocation:AggregateRoot<ViaId>
{
    public ViaLocationId ViaLocationId { get; private init; }
    public ViaLocationName ViaLocationName { get; private init; }
    public ViaLocationCapacity ViaLocationCapacity { get; private init; }


    public void UpdateName(string name)
    {
       new ViaLocationName(name);
    }
    public void SetLocationCapacity(int capacity)
    {
        new ViaLocationCapacity(capacity);
    }
    
}