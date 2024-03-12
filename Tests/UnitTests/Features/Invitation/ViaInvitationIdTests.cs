using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Invitation;

public class ViaInvitationIdTests
{


    public class ViaEventIdTests
    {
        [Fact]
        public void Create_ShouldGenerateNonEmptyGuid()
        {
            // Act
            var result = ViaInvitationId.Create();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Payload.Value);
        }
    }
}