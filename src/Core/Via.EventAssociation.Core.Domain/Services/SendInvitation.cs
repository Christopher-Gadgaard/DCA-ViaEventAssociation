using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Services;

public class SendInvitation
{
   
        private readonly IViaGuestRepository _guestRepository;
        private readonly IViaInvitationRepository _invitationRepository;
        private readonly IViaEventRepository _eventRepository;

        public SendInvitation(IViaGuestRepository guestRepository, IViaInvitationRepository invitationRepository,
            IViaEventRepository eventRepository)
        {
            _guestRepository = guestRepository;
            _invitationRepository = invitationRepository;
            _eventRepository = eventRepository;
        }

        public OperationResult SendInvitationToEvent(ViaInvitationId invitationId)
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

            var updateResult = _invitationRepository.Update(viaInvitation);
            if (updateResult == null)
            {
                return OperationResult.Failure(new List<OperationError> { new OperationError(ErrorCode.NotFound, "Update operation returned an unexpected null result.") });
            }
    
            return updateResult.IsFailure ? updateResult : OperationResult.Success();
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
        var eventResult = _eventRepository.GetById(viaInvitation.ViaEventId);
        if (!eventResult.IsSuccess)
        {
            return OperationResult.Failure(new List<OperationError>()
                {new OperationError(ErrorCode.NotFound, "Event not found")});
        }

        var viaEvent = eventResult.Payload;
        
        
        if (viaEvent.Status != ViaEventStatus.Active && viaEvent.Status != ViaEventStatus.Ready)

        {
            return OperationResult.Failure(new List<OperationError>()
                {new OperationError(ErrorCode.Conflict, "Cant send invitation. The event is not in a ready or active state.")});
        }

        if (viaEvent.IsFull())
        {
            return OperationResult.Failure(new List<OperationError>(){new OperationError(ErrorCode.Conflict, "The event is full.")});
        }
     

        return OperationResult.Success();
    }
}