using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.UpdateTitle;

public class ViaEventUpdateTitleTests
{
    [Fact]
    public void UpdateTitle_Success_WhenEventIsDraft()
    {
        // Arrange
        var viaEvent = ViaEventTestDataFactory.CreateDraftEvent();
        const string newTitle = "Scary Movie Night!";

        // Act
        var result = viaEvent.UpdateTitle(newTitle);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newTitle, viaEvent.Title?.Value);
        Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Remains Draft if it was Draft
    }

    [Fact]
    public void UpdateTitle_Success_RevertsToDraft_WhenEventIsReady()
    {
        // Arrange
        var viaEvent = ViaEventTestDataFactory.CreateReadyEvent();
        const string newTitle = "Graduation Gala";

        // Act
        var result = viaEvent.UpdateTitle(newTitle);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newTitle, viaEvent.Title?.Value);
        Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Reverts to Draft if it was Ready
    }

    [Theory]
    [InlineData("")]
    [InlineData("XY")]
    [InlineData(
        "This title is way too long and definitely exceeds the seventy-five character limit set by the domain rules.")]
    public void UpdateTitle_FailureDueToInvalidTitle(string invalidTitle)
    {
        // Arrange
        var viaEvent = ViaEventTestDataFactory.CreateDraftEvent();

        // Act
        var result = viaEvent.UpdateTitle(invalidTitle);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.InvalidInput);
    }

    [Fact]
    public void UpdateTitle_FailureDueToNonModifiableState_WhenEventIsActive()
    {
        // Arrange
        var viaEvent = ViaEventTestDataFactory.CreateActiveEvent();

        // Act
        var result = viaEvent.UpdateTitle("New Title");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.BadRequest);
    }

    [Fact]
    public void UpdateTitle_FailureDueToNonModifiableState_WhenEventIsCancelled()
    {
        // Arrange
        var viaEvent = ViaEventTestDataFactory.CreateCancelledEvent();

        // Act
        var result = viaEvent.UpdateTitle("New Title");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.BadRequest);
    }
}