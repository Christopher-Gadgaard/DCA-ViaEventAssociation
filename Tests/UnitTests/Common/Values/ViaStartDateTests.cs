using Via.EventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Common.Values;

public class ViaStartDateTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenStartDateIsValid()
    {
        // Arrange
        var validStartDate = new DateTime(2023, 1, 1, 9, 0, 0); // 9:00 AM is a valid start time

        // Act
        var result = ViaStartDate.Create(validStartDate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validStartDate, result.Payload.Value);
    }

    [Fact]
    public void Create_ShouldFail_WhenStartDateIsBefore8AM()
    {
        // Arrange
        var earlyStartDate = new DateTime(2023, 1, 1, 7, 59, 0); // 7:59 AM is too early

        // Act
        var result = ViaStartDate.Create(earlyStartDate);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput);
        var error = result.OperationErrors.FirstOrDefault();
        Assert.NotNull(error);
        Assert.Equal("Event cannot start before 08:00 AM.", error.Message);
    }
}