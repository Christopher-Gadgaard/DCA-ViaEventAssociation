using Via.EventAssociation.Core.Domain.Aggregates.Locations;
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
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        var result = location.UpdateLocationCapacity(10); 
        
        //Assert
        Assert.True(result.IsSuccess);
    }
    [Fact]
    
    public void Update_ShouldReturnSuccessWhenValid()
    {
        // Arrange
        var viaLocationId = ViaLocationId.Create().Payload;
        var viaLocationName = ViaLocationName.Create("Location Name").Payload;
        var viaLocationCapacity = ViaLocationCapacity.Create(100).Payload;
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        location.UpdateLocationCapacity(10); 
        
       //Assert
        Assert.Equal(10, location.Capacity.Value);
    }
    
    [Fact]
    
    public void Update_ShouldReturnFailureWhenEmpty()
    {
        // Arrange
        var viaLocationId = ViaLocationId.Create().Payload;
        var viaLocationName = ViaLocationName.Create("Location Name").Payload;
        var viaLocationCapacity = ViaLocationCapacity.Create(100).Payload;
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        var result = location.UpdateLocationCapacity(0); 
        
        //Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    [Fact]
    
    public void Update_CapacityTooHighShouldFail()
    {
        // Arrange
        var viaLocationId = ViaLocationId.Create().Payload;
        var viaLocationName = ViaLocationName.Create("Location Name").Payload;
        var viaLocationCapacity = ViaLocationCapacity.Create(100).Payload;
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        var result = location.UpdateLocationCapacity(101); 
        
        //Assert
        Assert.True(result.OperationErrors.Any());
    }
}