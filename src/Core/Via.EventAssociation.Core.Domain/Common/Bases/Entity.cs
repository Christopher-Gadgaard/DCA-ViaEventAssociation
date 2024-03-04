namespace Via.EventAssociation.Core.Domain.Common.Bases;

public abstract class Entity<TViaId>
{
    public TViaId Id { get; protected set; }

    protected Entity(TViaId id)
    {
        Id = id;
    }
    
    protected Entity(){}
}