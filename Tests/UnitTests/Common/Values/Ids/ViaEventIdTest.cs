using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Common.Values.Ids;

public class ViaEventIdTests
{
    [Fact]
    public void Create_ShouldGenerateNonEmptyGuid()
    {
        // Act
        var result = ViaEventId.Create();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Payload.Value);
    }
}