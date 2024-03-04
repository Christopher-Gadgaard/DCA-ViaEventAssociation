namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocationId
{
    public Guid Value { get; private init; }
    
    public ViaLocationId(Guid value)
    {
        Value = value;
    }
}