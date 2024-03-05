using Via.EventAssociation.Core.Domain.Aggregates.Locations;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

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
    [Fact]
     
    public void Create_ShouldReturnFailure()
    {
        // Arrange
        const int invalidId = 0;

        // Act
        var result = ViaLocationId.Create();

        // Assert
        Assert.True(result.OperationErrors.Any());
    }

}