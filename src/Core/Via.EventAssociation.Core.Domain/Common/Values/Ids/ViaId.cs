using Via.EventAssociation.Core.Domain.Common.Bases;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public abstract class ViaId: ValueObject
{
    public Guid Value { get; private init; }
    
    protected ViaId(Guid value)
    {
        Value = value;
    }
}