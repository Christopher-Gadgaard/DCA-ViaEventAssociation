using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event.MakePrivate;

public abstract class ViaEventMakePrivateTests
{
    public class S1
    {
        [Fact]
        public void InitializedPrivateAndStillPrivateAfterMakePrivate_Success_WhenEventIsDraft()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            
            // Act
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
            viaEvent.MakePrivate();

            // Assert
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        }
        
        [Fact]
        public void InitializedPrivateAndStillPrivateAfterMakePrivate_Success_WhenEventIsReady()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            
            // Act
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Ready)
                .Build();
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
            viaEvent.MakePrivate();

            // Assert
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        }
    }
    
    public class S2
    {
        [Fact]
        public void MakePrivate_Success_WhenEventIsDraftAndPublic()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            
            // Act
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithVisibility(ViaEventVisibility.Public)
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
            var result = viaEvent.MakePrivate();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        }
        
        [Fact]
        public void MakePrivate_Success_WhenEventIsReadyAndPublic()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            
            // Act
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithVisibility(ViaEventVisibility.Public)
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
            var result = viaEvent.MakePrivate();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        }
        
    }
    
    public class F1
    {
        [Fact]
        public void MakePrivate_Failure_WhenEventIsActive()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            
            // Act
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Active)
                .Build();
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
            var result = viaEvent.MakePrivate();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        }
    }
    
    public class F2
    {
        [Fact]
        public void MakePrivate_Failure_WhenEventIsCancelled()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            
            // Act
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Cancelled)
                .Build();
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
            var result = viaEvent.MakePrivate();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        }
    }
}