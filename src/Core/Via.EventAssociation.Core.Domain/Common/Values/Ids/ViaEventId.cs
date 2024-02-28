using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaEventId
{
    public Guid Value { get; private init; }

    private ViaEventId(Guid value)
    {
        Value = value;
    }

    public static OperationResult<ViaEventId> Create()
    {
        var id = new Guid();
        return new ViaEventId(id);
    }
}