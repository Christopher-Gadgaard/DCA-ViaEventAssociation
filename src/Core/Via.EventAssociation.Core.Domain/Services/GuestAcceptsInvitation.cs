using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Services;

public class GuestAcceptsInvitation
{
    
    IViaGuestRepository _guestRepository;
    IViaInvitationRepository _invitationRepository;
    IViaEventRepository _eventRepository;
    
    
    public GuestAcceptsInvitation(IViaGuestRepository guestRepository, IViaInvitationRepository invitationRepository, IViaEventRepository eventRepository)
    {
        _guestRepository = guestRepository;
        _invitationRepository = invitationRepository;
        _eventRepository = eventRepository;
    }
    
    public OperationResult AcceptInvitation(ViaInvitationId invitationId)
    {
        var invitationResult = _invitationRepository.GetById(invitationId);
        if (!invitationResult.IsSuccess)
            return invitationResult;
        
        var viaInvitation = invitationResult.Payload;
        
        var eventResult = ValidateEvent(viaInvitation);
        if (!eventResult.IsSuccess)
            return eventResult;
        
        var guestResult = ValidateGuest(viaInvitation);
        if (!guestResult.IsSuccess)
            return guestResult;

        viaInvitation.Accept();
        var updateResult = _invitationRepository.Update(viaInvitation);
        if (updateResult.IsFailure)
            return updateResult;
        
        return OperationResult.Success();
    }
    
    private OperationResult ValidateGuest(ViaInvitation viaInvitation)
    {
        var guestResult = _guestRepository.GetById(viaInvitation.ViaGuestId);
        if (!guestResult.IsSuccess)
            return OperationResult.Failure(new List<OperationError>(){new OperationError(ErrorCode.NotFound, "Guest not found")} );
        
        return OperationResult.Success();
    }
    
    private OperationResult ValidateEvent(ViaInvitation viaInvitation)
    {
        //TODO: check for capacity
        var eventResult = _eventRepository.GetById(viaInvitation.ViaEventId);
        if (!eventResult.IsSuccess)
            return OperationResult.Failure(new List<OperationError>(){new OperationError(ErrorCode.NotFound, "Event not found")} );
        
        return OperationResult.Success();
    }
}