using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaInvitationRequestId :ViaId
{
    public ViaInvitationRequestId(Guid value) : base(value)
    {
    }
    
    public static OperationResult<ViaInvitationRequestId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaInvitationRequestId(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}