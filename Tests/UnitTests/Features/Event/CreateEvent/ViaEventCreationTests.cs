using UnitTests.Common.Utilities;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event.CreateEvent;

public abstract class ViaEventCreationTests
{
    public class S1_S2_S3_S4
    {
         [Fact]
            public void TestCreateEvent_Success()
            {
                // Arrange
                var eventId = ViaEventId.Create();
                Assert.True(eventId.IsSuccess);
        
                // Act
                
                var result = ViaEvent.Create(eventId.Payload);
        
                // Assert
                Assert.True(result.IsSuccess);
                var viaEvent = result.Payload;
                Assert.Equal(eventId.Payload, viaEvent.Id);
                Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
                Assert.Equal(5, viaEvent.MaxGuests.Value);
                Assert.Equal("Working Title", viaEvent.Title?.Value);
                Assert.Equal("", viaEvent.Description?.Value);
                Assert.NotNull(viaEvent.DateTimeRange);
                Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
                Assert.Empty(viaEvent.Guests);
            }
    }
}