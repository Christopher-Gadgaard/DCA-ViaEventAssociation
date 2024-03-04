namespace Via.EventAssociation.Core.Domain.Common.Bases;

public abstract class AggregateRoot<TViaId> : Entity<TViaId>
{
    protected AggregateRoot(TViaId id) : base(id)
    {
        
    }
    
    protected AggregateRoot(){}
}