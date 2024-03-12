using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event;

public class ViaEventTestDataFactory
{
    
   private ViaEvent _event;

    private ViaEventTestDataFactory(ViaEventId id)
    {
        
        _event = ViaEvent.Create(id).Payload; 
    }

    public static ViaEventTestDataFactory Init(ViaEventId id)
    {
        return new ViaEventTestDataFactory(id);
    }

    public ViaEventTestDataFactory WithStatus(ViaEventStatus status)
    {
       
        if (status == ViaEventStatus.Active )
        {
            _event.UpdateStatus(ViaEventStatus.Ready); 
            _event.UpdateStatus(status);
        }
        else
        {
            _event.UpdateStatus(status);
        }
        return this;
    }

    public ViaEventTestDataFactory WithTitle(string title)
    {
        var titleResult = ViaEventTitle.Create(title);
        if (titleResult.IsSuccess)
        {
            _event.UpdateTitle(titleResult.Payload!);
        }
        return this;
    }

    public ViaEventTestDataFactory WithDescription(string description)
    {
        var descriptionResult = ViaEventDescription.Create(description);
        if (descriptionResult.IsSuccess)
        {
            _event.UpdateDescription(descriptionResult.Payload!);
        }
        return this;
    }

    public ViaEventTestDataFactory WithDateTimeRange(DateTime start, DateTime end)
    {
        var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);
        if (dateTimeRangeResult.IsSuccess)
        {
            _event.UpdateDateTimeRange(dateTimeRangeResult.Payload!);
        }
        return this;
    }

    public ViaEventTestDataFactory WithMaxGuests(int maxGuests)
    {
        var maxGuestsResult = ViaMaxGuests.Create(maxGuests);
        if (maxGuestsResult.IsSuccess)
        {
           // _event.UpdateMaxGuests(maxGuestsResult.Payload);
        }
        return this;
    }

    public ViaEvent Build()
    {
        return _event;
    }  
}