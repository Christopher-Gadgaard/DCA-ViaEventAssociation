using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.Values;

public class ViaMaxGuestsTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(25)]
    [InlineData(50)]
    public void Create_ShouldReturnSuccess_WhenGuestCountIsValid(int guestCount)
    {
        // Act
        var result = ViaMaxGuests.Create(guestCount);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(guestCount, result.Payload.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public void Create_ShouldFail_WhenGuestCountIsTooLow(int guestCount)
    {
        // Act
        var result = ViaMaxGuests.Create(guestCount);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput && e.Message == "Guest must be between 5 and 50 guests.");
    }

    [Theory]
    [InlineData(51)]
    [InlineData(100)]
    public void Create_ShouldFail_WhenGuestCountIsTooHigh(int guestCount)
    {
        // Act
        var result = ViaMaxGuests.Create(guestCount);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.InvalidInput && e.Message == "Guest must be between 5 and 50 guests.");
    }
}