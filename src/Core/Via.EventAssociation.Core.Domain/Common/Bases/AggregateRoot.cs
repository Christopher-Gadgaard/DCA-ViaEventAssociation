namespace Via.EventAssociation.Core.Domain.Common.Bases;

public abstract class AggregateRoot<ViaId> : Entity<ViaId>
{
    protected AggregateRoot(ViaId id) : base(id)
    {
        
    }
    
    protected AggregateRoot(){}
}