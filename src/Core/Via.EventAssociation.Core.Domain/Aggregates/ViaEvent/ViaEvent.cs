using Via.EventAssociation.Core.Domain.Aggregates.ViaEvent.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.ViaEvent.Values;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace Via.EventAssociation.Core.Domain.Aggregates.ViaEvent;

public class ViaEvent : AggregateRoot<ViaEventId>
{
    public ViaEventId ViaEventId { get; private init; }
    private ViaEventTitle _viaEventTitle;
    private ViaEventStatus _eventStatus;
    private ViaEventDescription _viaEventDescription;
}