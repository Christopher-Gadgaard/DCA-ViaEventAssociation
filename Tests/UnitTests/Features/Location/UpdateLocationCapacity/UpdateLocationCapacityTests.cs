using Xunit;
using Via.EventAssociation.Core.Domain.Aggregates.Locations;

// Assuming these namespaces exist and contain the necessary classes
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Location.UpdateLocationCapacity;

public class UpdateLocationCapacityTests
{
    [Fact]
    public void Update_ShouldReturnSuccess()
    {
        // Arrange
        var viaLocationId = ViaLocationId.Create().Payload;
        var viaLocationName = ViaLocationName.Create("Location Name").Payload;
        var viaLocationCapacity = ViaLocationCapacity.Create(100).Payload;
        
        var newCapacity = ViaLocationCapacity.Create(10).Payload;
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        var result = location.UpdateLocationCapacity(newCapacity); 
        
        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Update_ShouldReflectNewCapacityWhenValid()
    {
        // Arrange
        var viaLocationId = ViaLocationId.Create().Payload;
        var viaLocationName = ViaLocationName.Create("Location Name").Payload;
        var viaLocationCapacity = ViaLocationCapacity.Create(100).Payload;
        var newCapacity = ViaLocationCapacity.Create(10).Payload;
        
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        location.UpdateLocationCapacity(newCapacity); 
        
        // Assert
        Assert.Equal(10, location.Capacity.Value);
    }

    // Note: Adjustments for validation testing of ViaLocationCapacity creation
    [Fact]
    public void CreateLocationCapacity_ShouldFailWhenZero()
    {
        // Act
        var result = ViaLocationCapacity.Create(0); 
        
        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CreateLocationCapacity_HighCapacityShouldFail()
    {
        // Assuming there's a maximum valid capacity, e.g., 1000
        var result = ViaLocationCapacity.Create(1001); 
        
        // Assert
        Assert.True(result.IsFailure);
    }
}
