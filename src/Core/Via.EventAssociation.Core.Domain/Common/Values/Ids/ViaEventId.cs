using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaEventId : ViaId
{
    private ViaEventId(Guid value) : base(value)
    {
        
    }

    public static OperationResult<ViaEventId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaEventId(id);
    }

    
}