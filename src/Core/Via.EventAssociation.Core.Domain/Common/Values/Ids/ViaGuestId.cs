using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaGuestId :ViaId
{
    
    private ViaGuestId(Guid value):base(value)
    {
  
    }
    
    public static OperationResult<ViaGuestId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaGuestId(id);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}