using Via.EventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Common.Values;

public class ViaEndDateTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenEndDateIsValid()
    {
        // Arrange
        var validEndDate = new DateTime(2023, 1, 1, 21, 0, 0); // 9:00 PM is a valid end time

        // Act
        var result = ViaEndDate.Create(validEndDate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validEndDate, result.Payload.Value);
    }

    [Fact]
    public void Create_ShouldFail_WhenEndDateIsAfter10PM()
    {
        // Arrange
        var lateEndDate = new DateTime(2023, 1, 1, 22, 1, 0); // 10:01 PM is too late

        // Act
        var result = ViaEndDate.Create(lateEndDate);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
        var error = result.OperationErrors.FirstOrDefault();
        Assert.NotNull(error);
        Assert.Equal("Event cannot end after 22:00.", error.Message);
    }
}