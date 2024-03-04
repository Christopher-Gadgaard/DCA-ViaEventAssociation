using Via.EventAssociation.Core.Domain.Aggregates.Locations;

namespace UnitTests.Features.Location.Values;

public class ViaLocationCapacityTests
{
    [Fact]
    
    public void Create_ShouldReturnSuccess_WhenCapacityIsValid()
    {
        // Arrange
        const int validCapacity = 1;
        
        // Act
        var result = ViaLocationCapacity.Create(validCapacity);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validCapacity, result.Payload.Value);
    }
    [Fact]
    public void Create_ShouldFail_WhenCapacityIsTooHigh()
    {
        // Arrange
        const int tooHighCapacity = 101;
        
        // Act
        var result = ViaLocationCapacity.Create(tooHighCapacity);
        
        // Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]
    public void Create_ShouldFail_WhenCapacityIsTooLow()
    {
        // Arrange
        const int tooLowCapacity = -1;
        
        // Act
        var result = ViaLocationCapacity.Create(tooLowCapacity);
        
        // Assert
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public void Create_ShouldFail_WhenCapacityIsZero()
    {
        // Arrange
        const int zeroCapacity = 0;
        
        // Act
        var result = ViaLocationCapacity.Create(zeroCapacity);
        
        // Assert
        Assert.False(result.IsSuccess);
    }
}