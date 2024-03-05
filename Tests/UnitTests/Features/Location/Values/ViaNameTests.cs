using Via.EventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Features.Location.Values;

public class ViaNameTests
{
    [Fact]
    public void Create_ShouldReturnSuccess()
    {
        
        // Act
        var name = ViaName.Create("John");
        var result = name;
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    [Fact]
    public void Create_ShouldReturnFailureWhenEmpty()
    {
        // Arrange
        var emptyName = ViaName.Create("");
        
        // Act
        var result = emptyName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    [Fact]
    public void Create_ShouldReturnFailureWhenNull()
    {
        // Arrange
        var nullName = ViaName.Create(null);
        
        // Act
        var result = nullName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    [Fact]
    public void Create_ShouldReturnFailureWhenTooLong()
    {
        // Arrange
        var tooLongName = ViaName.Create("way to long name lorem ipsum dolor sit amet");
        
        // Act
        var result = tooLongName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    [Fact]
    
    public void ShouldReturnFailureWhenInvalid()
    {
        // Arrange
        var invalidName = ViaName.Create("John1");
        
        // Act
        var result = invalidName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    [Fact]
    
    public void ShouldReturnFailureWhenInvalid2()
    {
        // Arrange
        var invalidName = ViaName.Create("John!");
        
        // Act
        var result = invalidName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    [Fact]
    public void ShouldReturnFailureWhenTooShort()
    {
        // Arrange
        var tooShortName = ViaName.Create("J");
        
        // Act
        var result = tooShortName;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    [Fact]
    public void ShouldReturnSuccessWhenSize2()
    {
        // Arrange
        var tooShortName = ViaName.Create("Jo");
        
        // Act
        var result = tooShortName;
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    
}