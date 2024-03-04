using Via.EventAssociation.Core.Domain.Aggregates.Locations;

namespace UnitTests.Features.Location.UpdateLocationName;

public class UpdateLocationNameTest
{
    [Fact]
    public void Update_ShouldReturnSuccess()
    {
        // Arrange
        var ViaLocationId =  ViaLocationId.Create(1);
        // Act
        var location = new ViaLocation();
        
        location.UpdateName("New Name"); 
        
       
    }

}