using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaLocationId :ViaId
{
    public ViaLocationId(Guid value):base(value)
    {
      
    }
    public static OperationResult<ViaLocationId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaLocationId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}