using Xunit;
using Via.EventAssociation.Core.Domain.Aggregates.Locations;


// Assuming these namespaces exist and contain the necessary classes
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
        
        var newLocationName = ViaLocationName.Create("New Name").Payload;
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        var result = location.UpdateName(newLocationName); 
        
        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Update_ShouldReflectNewNameWhenValid()
    {
        // Arrange
        var viaLocationId = ViaLocationId.Create().Payload;
        var viaLocationName = ViaLocationName.Create("Location Name").Payload;
        var viaLocationCapacity = ViaLocationCapacity.Create(100).Payload;
        
        var newLocationName = ViaLocationName.Create("New Name").Payload;
        // Act
        var location = new ViaLocation(viaLocationId, viaLocationName, viaLocationCapacity);
        
        location.UpdateName(newLocationName); 
        
        // Assert
        Assert.Equal("New Name", location.Name.Value);
    }

    // Note: The next three tests assume that ViaLocationName.Create() will fail (return a failure OperationResult)
    // for invalid names. Therefore, they do not directly test UpdateName, but rather the creation of ViaLocationName.

    [Fact]
    public void CreateLocationName_ShouldFailWhenEmpty()
    {
        // Act
        var result = ViaLocationName.Create(""); 
        
        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CreateLocationName_TooLongShouldFail()
    {
        // Act
        var result = ViaLocationName.Create(new string('a', 76)); // Assuming 75 is the max length
        
        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void CreateLocationName_TooShortShouldFail()
    {
        // Act
        var result = ViaLocationName.Create("Ne");
        
        // Assert
        Assert.True(result.IsFailure);
    }
}
