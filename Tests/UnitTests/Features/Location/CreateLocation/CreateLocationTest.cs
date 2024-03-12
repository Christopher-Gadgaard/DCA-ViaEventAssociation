using Xunit;
using Via.EventAssociation.Core.Domain.Aggregates.Locations;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Location;

public class ViaLocationCreationTests
{
    [Fact]
    public void CreateLocation_WithAllValidInputs_ShouldSucceed()
    {
        // Arrange
        var locationId = ViaLocationId.Create().Payload;
        var locationName = ViaLocationName.Create("Valid Location Name").Payload;
        var locationCapacity = ViaLocationCapacity.Create(100).Payload;

        // Act
        var location = new ViaLocation(locationId, locationName, locationCapacity);

        // Assert
        Assert.NotNull(location);
        Assert.Equal("Valid Location Name", location.Name.Value);
        Assert.Equal(100, location.Capacity.Value);
    }

    [Fact]
    public void CreateLocation_WithNullName_ShouldUseDefaultName()
    {
        // Arrange
        var locationId = ViaLocationId.Create().Payload;
        var defaultName = "Location Name"; 
        var locationCapacity = ViaLocationCapacity.Create(100).Payload;

        // Act
        var location = new ViaLocation(locationId, null, locationCapacity);

        // Assert
        Assert.NotNull(location);
        Assert.Equal(defaultName, location.Name.Value);
    }

    [Fact]
    public void CreateLocation_WithNullCapacity_ShouldUseDefaultCapacity()
    {
        // Arrange
        var locationId = ViaLocationId.Create().Payload;
        var locationName = ViaLocationName.Create("Location With Default Capacity").Payload;
        var defaultCapacity = 20; 

        // Act
        var location = new ViaLocation(locationId, locationName, null);

        // Assert
        Assert.NotNull(location);
        Assert.Equal(defaultCapacity, location.Capacity.Value);
    }
    

}
