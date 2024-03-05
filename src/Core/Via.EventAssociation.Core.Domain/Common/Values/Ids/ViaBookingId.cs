using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaBookingId :ViaId
{
    private ViaBookingId(Guid value) : base(value)
    {
        
    }
    public static OperationResult<ViaBookingId> Create()
    {
        var id=Guid.NewGuid();
        return new ViaBookingId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}