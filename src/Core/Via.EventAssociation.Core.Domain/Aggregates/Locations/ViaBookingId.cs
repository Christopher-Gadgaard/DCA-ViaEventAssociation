using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaBookingId :ValueObject
{
    public Guid Value { get; private init; }
    
    public ViaBookingId(Guid value)
    {
        Value = value;
    }
    public static OperationResult<ViaBookingId> Create()
    {
        var id = new Guid();
        return new ViaBookingId(id);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}