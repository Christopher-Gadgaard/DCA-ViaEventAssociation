using Via.EventAssociation.Core.Domain.Aggregates.Locations;

namespace UnitTests.Features.Location.Values;

public class ViaLocationNameTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenNameIsValid()
    {
        // Arrange
        const string validName = "This is a valid location name that is within the allowed character limit.";

        // Act
        var result = ViaLocationName.Create(validName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validName, result.Payload.Value);
    }

    [Fact]
    public void Create_ShouldFail_WhenNameIsTooLong()
    {
        // Arrange
        var longName = new string('a', 76); 

        // Act
        var result = ViaLocationName.Create(longName);
        
        // Assert
        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public void Create_ShouldFail_WhenNameIsTooShort()
    {
        // Arrange
        const string shortName = "ab";

        // Act
        var result = ViaLocationName.Create(shortName);
        
        // Assert
        Assert.False(result.IsSuccess);
    }
}