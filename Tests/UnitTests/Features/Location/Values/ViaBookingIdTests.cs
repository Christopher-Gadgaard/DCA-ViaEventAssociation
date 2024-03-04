using Via.EventAssociation.Core.Domain.Aggregates.Locations;

namespace UnitTests.Features.Location.Values;

public class ViaBookingIdTests
{
    [Fact]
    
    public void Create_ShouldReturnSuccess()
    {
        // Arrange
        const int validId = 1;
        
        // Act
        var result = ViaBookingId.Create();
        
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}