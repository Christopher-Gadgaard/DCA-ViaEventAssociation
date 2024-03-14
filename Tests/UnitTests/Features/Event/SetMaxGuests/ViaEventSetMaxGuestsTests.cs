using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.SetMaxGuests;

public abstract class ViaEventSetMaxGuestsTests
{
    public class S1
    {
        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(50)]
        public void SetMaxGuestsLessThanPrevious_Success_WhenEventIsDraft(int newMaxGuests)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithMaxGuests(50)
                .Build();
            Assert.Equal(50, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            var maxGuestsResult = ViaMaxGuests.Create(newMaxGuests);
            Assert.True(maxGuestsResult.IsSuccess);

            // Act
            var result = viaEvent.SetMaxGuests(maxGuestsResult.Payload!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newMaxGuests, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Remains Draft if it was Draft
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(50)]
        public void SetMaxGuestsLessThanPrevious_Success_WhenEventIsReady(int newMaxGuests)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithMaxGuests(50)
                .WithStatus(ViaEventStatus.Ready)
                .Build();
            Assert.Equal(50, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
            var maxGuestsResult = ViaMaxGuests.Create(newMaxGuests);
            Assert.True(maxGuestsResult.IsSuccess);

            // Act
            var result = viaEvent.SetMaxGuests(maxGuestsResult.Payload!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newMaxGuests, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
        }
    }

    public class S2
    {
        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(50)]
        public void SetMaxGuestsMoreThanPrevious_Success_WhenEventIsDraft(int newMaxGuests)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build();
            Assert.Equal(5, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            var maxGuestsResult = ViaMaxGuests.Create(newMaxGuests);
            Assert.True(maxGuestsResult.IsSuccess);

            // Act
            var result = viaEvent.SetMaxGuests(maxGuestsResult.Payload!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newMaxGuests, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Remains Draft if it was Draft
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(50)]
        public void SetMaxGuestsMoreThanPrevious_Success_WhenEventIsReady(int newMaxGuests)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Ready)
                .Build();
            Assert.Equal(5, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
            var maxGuestsResult = ViaMaxGuests.Create(newMaxGuests);
            Assert.True(maxGuestsResult.IsSuccess);

            // Act
            var result = viaEvent.SetMaxGuests(maxGuestsResult.Payload!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newMaxGuests, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
        }
    }

    public class S3
    {
        [Theory]
        [InlineData(5, 5)]
        [InlineData(5, 10)]
        [InlineData(10, 25)]
        [InlineData(25, 50)]
        [InlineData(50, 50)]
        public void SetMaxGuestsMoreThanPrevious_Success_WhenEventIsActive(int initGuests, int newMaxGuests)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithMaxGuests(initGuests)
                .WithStatus(ViaEventStatus.Active)
                .Build();
            Assert.Equal(initGuests, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
            var maxGuestsResult = ViaMaxGuests.Create(newMaxGuests);
            Assert.True(maxGuestsResult.IsSuccess);

            // Act
            var result = viaEvent.SetMaxGuests(maxGuestsResult.Payload!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newMaxGuests, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        }
    }

    public class F1
    {
        [Theory]
        [InlineData(10, 5)]
        [InlineData(25, 10)]
        [InlineData(50, 25)]
        public void SetMaxGuestsLessThanPrevious_Failure_WhenEventIsActive(int initGuests, int newMaxGuests)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithMaxGuests(initGuests)
                .WithStatus(ViaEventStatus.Active)
                .Build();
            Assert.Equal(initGuests, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
            var maxGuestsResult = ViaMaxGuests.Create(newMaxGuests);
            Assert.True(maxGuestsResult.IsSuccess);

            // Act
            var result = viaEvent.SetMaxGuests(maxGuestsResult.Payload!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(initGuests, viaEvent.MaxGuests.Value);
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("Cannot reduce max guests for an active event."));
        }
    }

    public class F2
    {
        [Fact]
        public void SetMaxGuests_FailureDueToNonModifiableState_WhenEventIsCancelled()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithMaxGuests(50)
                .WithStatus(ViaEventStatus.Cancelled)
                .Build();
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
            var maxGuestsResult = ViaMaxGuests.Create(25);
            Assert.True(maxGuestsResult.IsSuccess);

            // Act
            var result = viaEvent.SetMaxGuests(maxGuestsResult.Payload!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The event cannot be modified in its current state."));
        }
    }

    public class F3
    {
        //TODO: Add when location is done. 
    }

    public class F4
    {
        [Theory]
        [InlineData(4)]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public void SetMaxGuests_FailureDueToInvalidInput_WhenLessThan5(int maxGuests)
        {
            // Arrange
            var maxGuestsResult = ViaMaxGuests.Create(maxGuests);
            Assert.True(maxGuestsResult.IsFailure);
            Assert.Contains(maxGuestsResult.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
            Assert.Contains(maxGuestsResult.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("Guest must be between 5 and 50 guests."));
        }
    }

    public class F5
    {
        [Theory]
        [InlineData(51)]
        [InlineData(52)]
        [InlineData(100)]
        [InlineData(1000)]
        public void SetMaxGuests_FailureDueToInvalidInput_WhenMoreThan50(int maxGuests)
        {
            // Arrange
            var maxGuestsResult = ViaMaxGuests.Create(maxGuests);
            Assert.True(maxGuestsResult.IsFailure);
            Assert.Contains(maxGuestsResult.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
            Assert.Contains(maxGuestsResult.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("Guest must be between 5 and 50 guests."));
        }
    }
}