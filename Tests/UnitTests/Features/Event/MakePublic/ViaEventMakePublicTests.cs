using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.MakePublic;

public abstract class ViaEventMakePublicTests
{
    public class S1
    {
        [Fact]
        public void MakePublic_Success_WhenEventIsDraft()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);

            // Act
            var result = viaEvent.MakePublic();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
        }
        
        [Fact]
        public void MakePublic_Success_WhenEventIsReady()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Ready)
                .Build();
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);

            // Act
            var result = viaEvent.MakePublic();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
        }
        
        [Fact]
        public void MakePublic_Success_WhenEventIsActive()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Active)
                .Build();
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);

            // Act
            var result = viaEvent.MakePublic();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
        }
    }
    
    public class F1
    {
        [Fact]
        public void MakePublic_Failure_WhenEventIsCancelled()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithStatus(ViaEventStatus.Cancelled)
                .Build();
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);

            // Act
            var result = viaEvent.MakePublic();

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The event cannot be modified in its current state."));
        }
    }
}