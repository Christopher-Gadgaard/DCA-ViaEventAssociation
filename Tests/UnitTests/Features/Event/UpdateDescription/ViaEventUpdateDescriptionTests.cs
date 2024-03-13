using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.UpdateDescription;

public abstract class ViaEventUpdateDescriptionTests
{
    public class S1_S2
    {
        [Theory]
        [InlineData("")]
        [InlineData(
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut. aliquip ex ea commodo consequat duis aute irure do")]
        public void UpdateDescription_Success_WhenEventIsDraft(string newDescription)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithDescription("Initial Description")
                .Build();
            var descriptionResult = ViaEventDescription.Create(newDescription);
            Assert.True(descriptionResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateDescription(descriptionResult.Payload!);


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
        [InlineData(
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut. aliquip ex ea commodo consequat duis aute irure do")]
        public void UpdateDescription_Success_RevertsToDraft_WhenEventIsReady(string newDescription)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithDescription("Initial Description")
                .WithStatus(ViaEventStatus.Ready)
                .Build();
            var descriptionResult = ViaEventDescription.Create(newDescription);
            Assert.True(descriptionResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateDescription(descriptionResult.Payload!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription, viaEvent.Description?.Value);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Reverts to Draft
        }
    }

    public class F1
    {
        [Theory]
        [InlineData(
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut. aliquip ex ea commodo consequat duis aute irure dod")]
        public void UpdateDescription_FailureDueToInvalidDescription(string invalidDescription)
        {
            // Arrange
            var descriptionResult = ViaEventDescription.Create(invalidDescription);

            // Assert
            Assert.True(descriptionResult.IsFailure);
            var error = descriptionResult.OperationErrors.FirstOrDefault();
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
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithDescription("Initial Description")
                .WithStatus(ViaEventStatus.Cancelled)
                .Build();
            var descriptionResult = ViaEventDescription.Create("New description");
            Assert.True(descriptionResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateDescription(descriptionResult.Payload!);

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
            var viaEventId = ViaEventId.Create();

            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithDescription("Initial Description")
                .WithStatus(ViaEventStatus.Active)
                .Build();

            var descriptionResult = ViaEventDescription.Create("New description");
            Assert.True(descriptionResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateDescription(descriptionResult.Payload!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The event cannot be modified in its current state."));
        }
    }
}