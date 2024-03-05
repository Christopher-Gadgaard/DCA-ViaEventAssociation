using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Location.Values;

public class ViaGuestIdTests
{
    [Fact]
    
    public void Create_ShouldReturnSuccess()
    {
        
        // Act
        var result = ViaGuestId.Create();
        
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}