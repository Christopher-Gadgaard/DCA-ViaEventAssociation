using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event;
public class ViaEvent : AggregateRoot<ViaEventId>
{
    private ViaEventTitle _title;
    private ViaEventDescription _description;
    private ViaStartDate _startDate;
    private ViaEndDate _endDate;
    private ViaMaxGuests _maxGuests;
    private ViaEventStatus _status;
    internal ViaEventStatus Status => _status;

    private ViaEvent(ViaEventId id, ViaEventTitle title, ViaEventDescription description, ViaStartDate startDate, ViaEndDate endDate, ViaMaxGuests maxGuests)
        : base(id)
    {
        _title = title;
        _description = description;
        _startDate = startDate;
        _endDate = endDate;
        _maxGuests = maxGuests;
        _status = ViaEventStatus.Draft;
    }
    
    public static OperationResult<ViaEvent> Create(ViaEventId id, ViaEventTitle title, ViaEventDescription description, ViaStartDate startDate, ViaEndDate endDate, ViaMaxGuests maxGuests)
    {
        return new ViaEvent(id, title, description, startDate, endDate, maxGuests);
    }
    
    }