using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event.Ready;

public class ViaEventReadyEventTests
{
    public class S1
    {
        [Fact]
        public void ReadyEvent_Success_WhenEventIsDraftAndComplete()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Some Title")
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            
            
            // Act
            var result = viaEvent.UpdateStatus(ViaEventStatus.Ready);
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
        }
    }
    
    public class F1
    {
        [Fact]
        public void ReadyEvent_Failure_WhenEventIsDraftAndIncomplete()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            
            // Act
            var result = viaEvent.UpdateStatus(ViaEventStatus.Ready);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
        }
    }
    
    public class F2
    {
        [Fact]
        public void ReadyEvent_Failure_WhenEventIsCancelled()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Test Title")
                .WithStatus(ViaEventStatus.Cancelled)
                .Build();
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
            
            // Act
            var result = viaEvent.UpdateStatus(ViaEventStatus.Ready);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
        }
    }
}