using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Utilities;
using Via.EventAssociation.Core.Domain.Common.Utilities.Interfaces;
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
    private List<ViaGuestId> _guests;

    private readonly ITimeProvider _timeProvider;

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
        ViaEventVisibility visibility = ViaEventVisibility.Private, 
        ITimeProvider? timeProvider = null)
        : base(id)
    {
        _timeProvider = timeProvider ?? new SystemTimeProvider();
        _title = title ?? ViaEventTitle.Create("Working Title").Payload;
        _description = description ?? ViaEventDescription.Create("").Payload;
        _dateTimeRange = dateTimeRange ?? ViaDateTimeRange
            .Create(DateTime.UtcNow.AddSeconds(30), DateTime.UtcNow.AddSeconds(30).AddHours(1))
            .Payload; //TODO: ASK TROELS ABOUT THIS
        _maxGuests = maxGuests ?? ViaMaxGuests.Create(5).Payload;
        _status = status;
        _visibility = visibility;
        _guests = new List<ViaGuestId>();
    }

    public static OperationResult<ViaEvent> Create(ViaEventId id)
    {
        return new ViaEvent(id);
    }
    
    internal static OperationResult<ViaEvent> Create(ViaEventId id, ITimeProvider timeProvider)
    {
        return new ViaEvent(id, timeProvider: timeProvider);
    }

    internal static OperationResult<ViaEvent> Create(
        ViaEventId id,
        ViaEventTitle? title = null,
        ViaEventDescription? description = null,
        ViaDateTimeRange? dateTimeRange = null,
        ViaMaxGuests? maxGuests = null,
        ViaEventStatus status = ViaEventStatus.Draft,
        ViaEventVisibility visibility = ViaEventVisibility.Private,
        ITimeProvider? timeProvider = null)
    {
        var viaEvent = new ViaEvent(id, title, description, dateTimeRange, maxGuests, status, visibility, timeProvider);
        return OperationResult<ViaEvent>.Success(viaEvent);
    }

    public OperationResult UpdateTitle(ViaEventTitle newTitle)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        _title = newTitle;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateDescription(ViaEventDescription newDescription)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        _description = newDescription;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateDateTimeRange(ViaDateTimeRange newDateTimeRange)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        _dateTimeRange = newDateTimeRange;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateStatus(ViaEventStatus newStatus)
    {
        if (newStatus == ViaEventStatus.Cancelled)
        {
            return TryCancelEvent();
        }

        return (_status, newStatus) switch
        {
            (ViaEventStatus.Draft, ViaEventStatus.Ready) => TryReadyEvent(),
            (ViaEventStatus.Ready, ViaEventStatus.Active) => TryActivateEvent(),
            _ => OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, $"Transitioning from '{_status}' to '{newStatus}' status is not supported.")
            })
        };
    }
    
    public OperationResult MakePublic()
    {
        if (_status == ViaEventStatus.Cancelled)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")
            });
        }
        
        _visibility = ViaEventVisibility.Public;
        return OperationResult.Success();
    }
    
    public OperationResult MakePrivate()
    {
        if (_status is ViaEventStatus.Cancelled or ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")
            });
        }
        
        _visibility = ViaEventVisibility.Private;
        return OperationResult.Success();
    }

    private OperationResult TryReadyEvent()
    {
        if (!IsEventDataComplete())
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Event data is incomplete, cannot transition to Ready.")
            });
        }

        _status = ViaEventStatus.Ready;
        return OperationResult.Success();
    }

    private OperationResult TryActivateEvent()
    {
        if (_timeProvider.Now >= _dateTimeRange!.StartValue)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Cannot activate past events.")
            });
        }

        _status = ViaEventStatus.Active;
        return OperationResult.Success();
    }

    private OperationResult TryCancelEvent()
    {
        _status = ViaEventStatus.Cancelled;
        return OperationResult.Success();
    }

    private bool IsEventDataComplete()
    {
        var titleIsInitialized = _title != null;
        var descriptionIsInitialized = _description != null;
        var dateTimeRangeIsInitialized = _dateTimeRange != null;
        var maxGuestsIsInitialized = _maxGuests != null;

        return titleIsInitialized && descriptionIsInitialized && dateTimeRangeIsInitialized && maxGuestsIsInitialized;
    }

    private OperationResult CheckModifiableState()
    {
        return _status is ViaEventStatus.Active or ViaEventStatus.Cancelled
            ? OperationResult.Failure(new List<OperationError>
                {new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")})
            : OperationResult.Success();
    }

    private OperationResult IfReadyRevertToDraft()
    {
        if (_status == ViaEventStatus.Ready)
        {
            _status = ViaEventStatus.Draft;
        }

        return OperationResult.Success();
    }
}