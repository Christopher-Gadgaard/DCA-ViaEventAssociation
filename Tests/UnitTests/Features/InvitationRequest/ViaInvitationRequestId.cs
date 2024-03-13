using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.InvitationRequest;


    public class ViaInvitationRequestIdTests
    {
        [Fact]
        public void Create_ShouldGenerateNonEmptyGuid()
        {
            // Act
            var result = ViaInvitationRequestId.Create();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Payload.Value);
        
    }
}