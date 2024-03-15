﻿using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Services;

public class GuestDeclinesInvitation
{
    private readonly IViaInvitationRepository _invitationRepository;
    private readonly IViaEventRepository _eventRepository;
    private readonly IViaGuestRepository _guestRepository;
    
    public GuestDeclinesInvitation(IViaInvitationRepository invitationRepository, IViaEventRepository eventRepository, IViaGuestRepository guestRepository)
    {
        _invitationRepository = invitationRepository;
        _eventRepository = eventRepository;
        _guestRepository = guestRepository;
    }
    
    public OperationResult DeclineInvitation(ViaInvitationId invitationId)
    {
        var invitationResult = _invitationRepository.GetById(invitationId);
        if (!invitationResult.IsSuccess)
        {
            return invitationResult;
        }

        var viaInvitation = invitationResult.Payload;

        var eventResult = ValidateEvent(viaInvitation);
        if (!eventResult.IsSuccess)
            return eventResult;
        
        var guestResult = ValidateGuest(viaInvitation);
        if (!guestResult.IsSuccess)
            return guestResult;

     
     
        viaInvitation.Reject(); 


        var updateResult = _invitationRepository.Update(viaInvitation);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

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