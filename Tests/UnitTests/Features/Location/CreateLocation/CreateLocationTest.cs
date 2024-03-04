using Via.EventAssociation.Core.Domain.Aggregates.Locations;

namespace UnitTests.Features.Location.CreateLocation;

public class CreateLocationTest
{
    [Fact]

    public void Create_ShouldReturnSuccess()
    {
        // Arrange
        const int validId = 1;

        // Act
        var result = ViaLocationId.Create();


        // Assert
        Assert.True(result.IsSuccess);
    }

}