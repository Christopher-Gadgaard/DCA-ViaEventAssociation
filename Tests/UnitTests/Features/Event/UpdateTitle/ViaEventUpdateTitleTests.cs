using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
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
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithTitle("Initial Title")
                .Build();

            var titleResult = ViaEventTitle.Create(newTitle);
            Assert.True(titleResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateTitle(titleResult.Payload!);

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
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithTitle("Initial Title")
                .WithStatus(ViaEventStatus.Ready)
                .Build();

            var titleResult = ViaEventTitle.Create(newTitle);
            Assert.True(titleResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateTitle(titleResult.Payload!);

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
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithTitle("Initial Title")
                .Build();

            // Act
            // This will fail to create a title, so we check for failure directly
            var titleResult = ViaEventTitle.Create(invalidTitle);

            // Assert
            Assert.True(titleResult.IsFailure);
            Assert.Contains(titleResult.OperationErrors, error => error.Code == ErrorCode.InvalidInput);
            Assert.Contains(titleResult.OperationErrors,
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
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithTitle("Initial Title")
                .WithStatus(ViaEventStatus.Active)
                .Build();

            var titleResult = ViaEventTitle.Create("New Title");
            Assert.True(titleResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateTitle(titleResult.Payload!);

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
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithTitle("Initial Title")
                .WithStatus(ViaEventStatus.Cancelled)
                .Build();

            var titleResult = ViaEventTitle.Create("New Title");
            Assert.True(titleResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateTitle(titleResult.Payload!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The event cannot be modified in its current state."));
        }
    }
}