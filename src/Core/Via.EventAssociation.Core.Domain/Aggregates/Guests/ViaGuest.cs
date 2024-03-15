using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;

public class ViaGuest:AggregateRoot<ViaGuestId>
{
    
    private ViaGuestName _viaGuestName;
    private ViaEmail _viaEmail;
    private ICollection<ViaInvitationRequest> _invitationRequests;
    private ICollection<ViaInvitation> _invitations;
    internal new ViaGuestId Id => base.Id;
    internal ViaGuestName ViaGuestName => _viaGuestName;
    internal ViaEmail ViaEmail => _viaEmail;
    internal ICollection<ViaInvitationRequest> InvitationRequests => _invitationRequests;
    
 
    public ViaGuest( ViaGuestId viaGuestId, ViaGuestName viaGuestName, ViaEmail viaEmail):base(viaGuestId)
    {
        _viaGuestName = viaGuestName;
        _viaEmail = viaEmail;
    }
    
    public static OperationResult<ViaGuest> Create(ViaGuestId viaGuestId, ViaGuestName viaGuestName, ViaEmail viaEmail)
    {
        return new ViaGuest(viaGuestId, viaGuestName, viaEmail);
    }
    public void AddRequest(ViaInvitationRequest request)
    {
        _invitationRequests.Add(request);
    }
    public void AddInvitation(ViaInvitation invitation)
    {
        _invitations.Add(invitation);
    }
}