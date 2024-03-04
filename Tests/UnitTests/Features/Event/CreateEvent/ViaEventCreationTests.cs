using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event.CreateEvent;

public class ViaEventCreationTests
{
    [Fact]
    public void Create_ShouldReturnSuccessResult_WithValidParameters()
    {
        // Arrange
        var id = ViaEventId.Create().Payload; 
        var title = ViaEventTitle.Create("Sample Event Title").Payload; 
        var description = ViaEventDescription.Create("This is a sample event description.").Payload; 
        var startDate = ViaStartDate.Create(new DateTime(2023, 1, 1, 10, 0, 0)).Payload; 
        var endDate = ViaEndDate.Create(new DateTime(2023, 1, 1, 12, 0, 0)).Payload; 
        var maxGuests = ViaMaxGuests.Create(50).Payload; 

        // Act
        var result = ViaEvent.Create(id, title, description, startDate, endDate, maxGuests);


        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(id, result.Payload.Id);
        Assert.Equal(ViaEventStatus.Draft, result.Payload.Status); 
    }
}