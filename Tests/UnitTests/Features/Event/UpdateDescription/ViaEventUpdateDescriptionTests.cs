using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.UpdateDescription;

public abstract class ViaEventUpdateDescriptionTests
{
    public class S1_S2
    {
        [Theory]
        [InlineData("")]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut. aliquip ex ea commodo consequat duis aute irure do")]
        public void UpdateDescription_Success_WhenEventIsDraft(string newDescription)
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateDraftEvent();
            
            // Act
            var result = viaEvent.UpdateDescription(newDescription);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription, viaEvent.Description?.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Remains Draft if it was Draft
        }
    }
    
    public class S3
    {
        [Theory]
        [InlineData("")]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut. aliquip ex ea commodo consequat duis aute irure do")]
        public void UpdateDescription_Success_RevertsToDraft_WhenEventIsReady(string newDescription)
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateReadyEvent();
            
            // Act
            var result = viaEvent.UpdateDescription(newDescription);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription, viaEvent.Description?.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Reverts to Draft
        }
    }

    public class F1
    {
        [Theory]
        [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut. aliquip ex ea commodo consequat duis aute irure dod")]
        public void UpdateDescription_FailureDueToInvalidDescription(string invalidDescription)
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateDraftEvent();
            
            // Act
            var result = viaEvent.UpdateDescription(invalidDescription);
            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
            var error = result.OperationErrors.FirstOrDefault();
            Assert.NotNull(error);
            Assert.Equal($"Description cannot exceed 250 characters. {invalidDescription.Length}/250", error.Message);
        }
    }
    
    public class F2
    {
        [Fact]
        public void UpdateDescription_FailureDueToNonModifiableState_WhenEventIsCancelled()
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateCancelledEvent();
            
            // Act
            var result = viaEvent.UpdateDescription("New description");
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
        [Fact]
        public void UpdateDescription_FailureDueToNonModifiableState_WhenEventIsActive()
        {
            // Arrange
            var viaEvent = ViaEventTestDataFactory.CreateActiveEvent();
            
            // Act
            var result = viaEvent.UpdateDescription("New description");
            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The event cannot be modified in its current state."));
        }
    }
}