using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Event.Values;

public class ViaEventTitleTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenTitleIsValid()
    {
        // Arrange
        const string validTitle = "Valid Event Title";

        // Act
        var result = ViaEventTitle.Create(validTitle);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validTitle, result.Payload.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_ShouldFail_WhenTitleIsNullOrEmpty(string title)
    {
        // Act
        var result = ViaEventTitle.Create(title);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
    }

    [Fact]
    public void Create_ShouldFail_WhenTitleIsTooShort()
    {
        // Arrange
        const string shortTitle = "No";

        // Act
        var result = ViaEventTitle.Create(shortTitle);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
    }

    [Fact]
    public void Create_ShouldFail_WhenTitleIsTooLong()
    {
        // Arrange
        var longTitle = new string('a', 76); 

        // Act
        var result = ViaEventTitle.Create(longTitle);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
    }
}