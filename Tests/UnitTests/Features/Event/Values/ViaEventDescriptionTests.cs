using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.Values;

public class ViaEventDescriptionTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenDescriptionIsValid()
    {
        // Arrange
        const string validDescription = "This is a valid event description that is within the allowed character limit.";

        // Act
        var result = ViaEventDescription.Create(validDescription);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validDescription, result.Payload.Value);
    }

    [Fact]
    public void Create_ShouldFail_WhenDescriptionIsTooLong()
    {
        // Arrange
        var longDescription = new string('a', 251); 

        // Act
        var result = ViaEventDescription.Create(longDescription);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
        var error = result.OperationErrors.FirstOrDefault();
        Assert.NotNull(error);
        Assert.Equal($"Description cannot exceed 250 characters. {longDescription.Length}/250", error.Message);
    }
}