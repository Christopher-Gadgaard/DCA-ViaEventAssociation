using UnitTests.Common.Utilities;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.Ready;

public abstract class ViaEventReadyEventTests
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
    
    public class F1_F4
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
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The title must be changed from the default."));
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
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("Transitioning from 'Cancelled' to 'Ready' status is not supported."));
        }
    }
    
    public class F3
    {
        [Fact]
        public void ReadyEvent_Failure_WhenEventIsInThePast()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var fakeTimeProvider = new FakeTimeProvider( DateTime.Now.AddDays(-2));
            var dateTimeRangeResult = ViaDateTimeRange.Create(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1).AddHours(1), fakeTimeProvider);
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Test Title")
                .WithStatus(ViaEventStatus.Draft)
                .WithDateTimeRange(dateTimeRangeResult.Payload)
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            
            // Act
            var result = viaEvent.UpdateStatus(ViaEventStatus.Ready);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The start time cannot be in the past."));
        }
        
    }
}