using Via.EventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Features.Location.Values;

public class ViaGuestNameTests
{
    [Fact]
     
    public void Create_ShouldReturnSuccess()
    {
        
        // Act
        var result = ViaGuestName.Create("John", "Doe");
        
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    
    public void Create_ShouldReturnFailureWhenEmpty()
    {
        // Arrange
        var emptyName = ViaGuestName.Create("", "");
        
        // Act
        var result = emptyName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    [Fact]
    
    public void Create_ShouldReturnFailureWhenNull()
    {
        // Arrange
        var nullName = ViaGuestName.Create(null, null);
        
        // Act
        var result = nullName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]

    public void Create_ShouldReturnFailureWhenTooLong()
    {
        // Arrange
        var tooLongName = ViaGuestName.Create("way to long name lorem ipsum dolor sit amet", "way to long name lorem ipsum dolor sit amet");

        // Act
        var result = tooLongName;

        // Assert
        Assert.True(result.OperationErrors.Any());

    }
    
    [Fact]
    
    public void Create_ShouldReturnFailureWhenInvalid()
    {
        // Arrange
        var invalidName = ViaGuestName.Create("invalid_name", "invalid_name");
        
        // Act
        var result = invalidName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
}