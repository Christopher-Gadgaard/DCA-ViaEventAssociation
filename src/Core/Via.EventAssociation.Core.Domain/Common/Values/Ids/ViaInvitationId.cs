using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaInvitationId:ViaId
{
    public ViaInvitationId(Guid value) : base(value)
    {
    }
    public static OperationResult<ViaInvitationId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaInvitationId(id);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}