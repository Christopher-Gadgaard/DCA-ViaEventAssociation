using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.UpdateTitle;

public abstract class ViaEventUpdateTitleTests
{
    public class S1
    {
        [Theory]
        [InlineData("Scary Movie Night!")]
        [InlineData("Graduation Gala")]
        [InlineData("VIA Hackathon")]
        public void UpdateTitle_Success_WhenEventIsDraft(string newTitle)
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateDraftEvent();

            // Act
            var result = viaEvent.UpdateTitle(newTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle, viaEvent.Title?.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Remains Draft if it was Draft
        }
    }

    public class S2
    {
        [Theory]
        [InlineData("Scary Movie Night!")]
        [InlineData("Graduation Gala")]
        [InlineData("VIA Hackathon")]
        public void UpdateTitle_Success_RevertsToDraft_WhenEventIsReady(string newTitle)
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateReadyEvent();

            // Act
            var result = viaEvent.UpdateTitle(newTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle, viaEvent.Title?.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Reverts to Draft
        }
    }

    public class F1_F2_F3_F4
    {
        [Theory]
        [InlineData("")]
        [InlineData("XY")]
        [InlineData(
            "This title is way too long and definitely exceeds the seventy-five character limit set by the domain rules.")]
        [InlineData(null)]
        public void UpdateTitle_FailureDueToInvalidTitle(string invalidTitle)
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateDraftEvent();

            // Act
            var result = viaEvent.UpdateTitle(invalidTitle);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.InvalidInput);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("Title must be between 3 and 75 characters long."));
        }
    }

    public class F5
    {
        [Fact]
        public void UpdateTitle_FailureDueToNonModifiableState_WhenEventIsActive()
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateActiveEvent();

            // Act
            var result = viaEvent.UpdateTitle("New Title");

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The event cannot be modified in its current state."));
        }
    }

    public class F6
    {
        [Fact]
        public void UpdateTitle_FailureDueToNonModifiableState_WhenEventIsCancelled()
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateCancelledEvent();

            // Act
            var result = viaEvent.UpdateTitle("New Title");

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.BadRequest);
        }
    }
}