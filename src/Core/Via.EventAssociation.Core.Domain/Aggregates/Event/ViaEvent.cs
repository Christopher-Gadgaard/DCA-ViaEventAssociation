using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event;

public class ViaEvent : AggregateRoot<ViaEventId>
{
    private ViaEventTitle? _title;
    private ViaEventDescription? _description;
    private ViaDateTimeRange? _dateTimeRange;
    private ViaMaxGuests _maxGuests;
    private ViaEventStatus _status;
    private ViaEventVisibility _visibility;
    private List<ViaGuestId> _guests = new();

    internal new ViaEventId Id => base.Id;
    internal ViaEventTitle? Title => _title;
    internal ViaEventDescription? Description => _description;
    internal ViaDateTimeRange? DateTimeRange => _dateTimeRange;
    internal ViaMaxGuests MaxGuests => _maxGuests;
    internal ViaEventStatus Status => _status;
    internal ViaEventVisibility Visibility => _visibility;
    internal IEnumerable<ViaGuestId> Guests => _guests;

    private ViaEvent(
        ViaEventId id,
        ViaEventTitle? title = null,
        ViaEventDescription? description = null,
        ViaDateTimeRange? dateTimeRange = null,
        ViaMaxGuests? maxGuests = null,
        ViaEventStatus status = ViaEventStatus.Draft,
        ViaEventVisibility visibility = ViaEventVisibility.Private)
        : base(id)
    {
        _title = title ?? ViaEventTitle.Create("Working Title").Payload;
        _description = description ?? ViaEventDescription.Create("").Payload;
        _dateTimeRange = dateTimeRange;
        _maxGuests = maxGuests ?? ViaMaxGuests.Create(5).Payload;
        _status = status;
        _visibility = visibility;
    }

    public static OperationResult<ViaEvent> Create(ViaEventId id)
    {
        return new ViaEvent(id);
    }
    
    public static OperationResult<ViaEvent> Create(
        ViaEventId id,
        ViaEventTitle? title = null,
        ViaEventDescription? description = null,
        ViaDateTimeRange? dateTimeRange = null,
        ViaMaxGuests? maxGuests = null,
        ViaEventStatus status = ViaEventStatus.Draft,
        ViaEventVisibility visibility = ViaEventVisibility.Private)
    {
        var viaEvent = new ViaEvent(id, title, description, dateTimeRange, maxGuests, status, visibility);
        
        return OperationResult<ViaEvent>.Success(viaEvent);
    }

    public OperationResult UpdateTitle(string newTitle)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        var titleUpdateResult = ViaEventTitle.Create(newTitle);
        if (titleUpdateResult.IsFailure)
        {
            return OperationResult.Failure(titleUpdateResult.OperationErrors);
        }

        _title = titleUpdateResult.Payload!;

        // If the event is in Ready status, revert it to Draft upon changing the title
        if (_status == ViaEventStatus.Ready)
        {
            _status = ViaEventStatus.Draft;
        }

        return OperationResult.Success();
    }


    // Check if is in a modifiable state
    private OperationResult CheckModifiableState()
    {
        return _status is ViaEventStatus.Active or ViaEventStatus.Cancelled
            ? OperationResult.Failure(new List<OperationError>
                {new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")})
            : OperationResult.Success();
    }
}