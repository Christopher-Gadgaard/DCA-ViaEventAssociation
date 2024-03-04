using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocationId :ValueObject
{
    public Guid Value { get; private init; }
    
    public ViaLocationId(Guid value)
    {
        Value = value;
    }
    public static OperationResult<ViaLocationId> Create()
    {
        var id = new Guid();
        return new ViaLocationId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}