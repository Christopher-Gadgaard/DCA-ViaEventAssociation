using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Services;

public class DeclineInvitationRequest
{
    private readonly IViaInvitationRequestRepository _viaInvitationRequestRepository;
    private readonly IViaEventRepository _viaEventRepository;
    private readonly IViaGuestRepository _viaGuestRepository;
    
    public DeclineInvitationRequest(IViaInvitationRequestRepository viaInvitationRequestRepository, IViaEventRepository viaEventRepository, IViaGuestRepository viaGuestRepository)
    {
        _viaInvitationRequestRepository = viaInvitationRequestRepository;
        _viaEventRepository = viaEventRepository;
        _viaGuestRepository = viaGuestRepository;
    }
    
    public async Task<OperationResult> DeclineRequest(ViaInvitationRequestId invitationRequestId)
    {
        var invitationRequestResult = _viaInvitationRequestRepository.GetById(invitationRequestId);
        if (!invitationRequestResult.IsSuccess)
            return invitationRequestResult;

        var viaInvitationRequest = invitationRequestResult.Payload;

        var eventResult = ValidateEvent(viaInvitationRequest);
        if (!eventResult.IsSuccess)
            return eventResult;

        var guestResult = ValidateGuest(viaInvitationRequest);
        if (!guestResult.IsSuccess)
            return guestResult;

        viaInvitationRequest.Reject();
        var updateResult = _viaInvitationRequestRepository.Update(viaInvitationRequest);
        if (updateResult.IsFailure)
            return updateResult;

        return OperationResult.Success();
    }
    private OperationResult ValidateGuest(ViaInvitationRequest viaInvitation)
    {
        var guestResult = _viaGuestRepository.GetById(viaInvitation.ViaGuestId);
        if (!guestResult.IsSuccess)
            return OperationResult.Failure(new List<OperationError>(){new OperationError(ErrorCode.NotFound, "Guest not found")} );

        return OperationResult.Success();
    }

    private OperationResult ValidateEvent(ViaInvitationRequest viaInvitation)
    {
        var eventResult = _viaEventRepository.GetById(viaInvitation.ViaEventId);
        if (!eventResult.IsSuccess)
        {
            return OperationResult.Failure(new List<OperationError>()
                { new OperationError(ErrorCode.NotFound, "Event not found") });
        }

        var viaEvent = eventResult.Payload;

        if (viaEvent.Status != ViaEventStatus.Ready || viaEvent.Status != ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>()
                { new OperationError(ErrorCode.Conflict, "Cant decline invitation; The event is not in a ready or active state.") });
        }
    

        if (viaEvent.Visibility != ViaEventVisibility.Private)
        {
            return OperationResult.Failure(new List<OperationError>()
                { new OperationError(ErrorCode.Conflict, "The event is not private.") });
        }

        return OperationResult.Success();
        //  to check if the event is full
    }
}