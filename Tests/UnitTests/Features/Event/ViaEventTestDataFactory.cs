using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event;

public abstract class ViaEventTestDataFactory
{
    public static ViaEvent CreateDraftEvent()
    {
        var id = ViaEventId.Create();
        return ViaEvent.Create(id.Payload, status: ViaEventStatus.Draft).Payload;
    }

    public static ViaEvent CreateReadyEvent()
    {
        var id = ViaEventId.Create();
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateValidDateRange();
        var dateTimeRange = ViaDateTimeRange.Create(start, end).Payload;
        var title = ViaEventTitle.Create("Ready Event Title").Payload;
        var description = ViaEventDescription.Create("A well-prepared event").Payload;
        var maxGuests = ViaMaxGuests.Create(50).Payload;
        return ViaEvent.Create(id.Payload, title, description, dateTimeRange, maxGuests, ViaEventStatus.Ready, ViaEventVisibility.Public).Payload;
    }

    public static ViaEvent CreateActiveEvent()
    {
        var id = ViaEventId.Create();
        return ViaEvent.Create(id.Payload, status: ViaEventStatus.Active).Payload;
    }

    public static ViaEvent CreateCancelledEvent()
    {
        var id = ViaEventId.Create();
        return ViaEvent.Create(id.Payload, status: ViaEventStatus.Cancelled).Payload;
    }
}