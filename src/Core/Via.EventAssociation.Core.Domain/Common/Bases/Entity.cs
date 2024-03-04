namespace Via.EventAssociation.Core.Domain.Common.Bases;

public abstract class Entity<ViaId>
{
    public ViaId Id { get; }

    protected Entity(ViaId id)
    {
        Id = id;
    }
    
    protected Entity(){}
}