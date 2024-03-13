using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event;

public class ViaInvitationRequest
{
    private ViaInvitationRequestId _id;
    private ViaEventId _viaEventId;
    private ViaGuestId _viaGuestId;
    private ViaInvitationStatus _status;
    
    internal ViaInvitationRequestId Id => _id;
    internal ViaEventId ViaEventId => _viaEventId;
    internal ViaGuestId ViaGuestId => _viaGuestId;
    internal ViaInvitationStatus Status => _status;
    
    public ViaInvitationRequest(ViaInvitationRequestId id, ViaEventId viaEventId, ViaGuestId viaGuestId)
    {
        _id = id;
        _viaEventId = viaEventId;
        _viaGuestId = viaGuestId;
        _status = ViaInvitationStatus.Pending;
    }
    public static OperationResult<ViaInvitationRequest> Create(ViaInvitationRequestId invitationRequestId, ViaEventId viaEventId, ViaGuestId viaGuestId)
    {
        return new ViaInvitationRequest( invitationRequestId,viaEventId, viaGuestId);
    }
    public OperationResult<ViaInvitationRequest> Accept()
    {
        _status = ViaInvitationStatus.Accepted;
        return this;
    }
    public OperationResult<ViaInvitationRequest> Reject()
    {
        _status = ViaInvitationStatus.Rejected;
        return this;
    }
}
