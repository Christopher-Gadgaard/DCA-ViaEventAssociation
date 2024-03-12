using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event;

public class ViaInvitation
{
    
    private ViaInvitationId _id;
    private ViaEventId _viaEventId;
    private ViaGuestId _viaGuestId;
    private ViaInvitationStatus _status;
    
    internal ViaInvitationId Id => _id;
    internal ViaEventId ViaEventId => _viaEventId;
    internal ViaGuestId ViaGuestId => _viaGuestId;
    internal ViaInvitationStatus Status => _status;
    public ViaInvitation(ViaInvitationId id, ViaEventId viaEventId, ViaGuestId viaGuestId)
  
    {
        _id = id;
        _viaEventId = viaEventId;
        _viaGuestId = viaGuestId;
        _status = ViaInvitationStatus.Pending;
    }
    public static OperationResult<ViaInvitation> Create(ViaInvitationId invitationId, ViaEventId viaEventId, ViaGuestId viaGuestId)
    {
        return new ViaInvitation( invitationId,viaEventId, viaGuestId);
    }
}