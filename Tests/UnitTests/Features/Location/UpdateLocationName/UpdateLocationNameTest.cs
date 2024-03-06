using Via.EventAssociation.Core.Domain.Aggregates.Locations;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Location.UpdateLocationName;

public class UpdateLocationNameTest
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
        
        var result = location.UpdateName("New name"); 
        
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
        
        location.UpdateName("New Name"); 
        
       //Assert
        Assert.Equal("New Name", location.Name.Value);
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
        
        var result = location.UpdateName(""); 
        
        //Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    [Fact]
    
    public void Update_NameTooLongShouldFail()
    {
        // Arrange
        var viaLocationId = ViaLocationId.Create().Payload;
        var viaLocationName = ViaLocationName.Create("Location Name").Payload;
        var viaLocationCapacity = ViaLocationCapacity.Create(100).Payload;
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        var result = location.UpdateName("New Name lorem ipsum Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley"); 
        
        //Assert
        Assert.True(result.OperationErrors.Any());
    }
    [Fact]
    
    public void Update_NameTooShortShouldFail()
    {
        // Arrange
        var viaLocationId = ViaLocationId.Create().Payload;
        var viaLocationName = ViaLocationName.Create("Location Name").Payload;
        var viaLocationCapacity = ViaLocationCapacity.Create(100).Payload;
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        var result = location.UpdateName("Ne"); 
        
        //Assert
        Assert.True(result.OperationErrors.Any());
    }
}