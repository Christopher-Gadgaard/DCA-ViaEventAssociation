using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Invitation;

public class CreateInvitationTest
{


    [Fact]
    public void Create_WithValidIds_ShouldReturnSuccess()
    {
        // Arrange
        var invitationId = ViaInvitationId.Create().Payload;
        var eventId =  ViaEventId.Create().Payload;
        var guestId =  ViaGuestId.Create().Payload;
        

        // Act
        var result = ViaInvitation.Create(invitationId,eventId, guestId);

        // Assert
        Assert.True(result.IsSuccess, "Creation of invitation should succeed.");
      
       
    }
    
    
    [Fact]
    public void Create_ShouldReturnSuccessWithExistingId()
    {
        // Arrange
        var invitationId = ViaInvitationId.Create().Payload;
        var eventId =  ViaEventId.Create().Payload;
        var guestId =  ViaGuestId.Create().Payload;

        // Act
        var result = ViaInvitation.Create(invitationId, eventId, guestId);

        // Assert
        var invitation = result.Payload;
        Assert.NotNull(invitation.Id);
    }
    
    [Fact]
    public void Create_WithValidIds_ShouldSetCorrectProperties()
    {
        // Arrange
        var invitationId =ViaInvitationId.Create().Payload;
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;

        // Act
        var result = ViaInvitation.Create(invitationId, eventId, guestId);
        var invitation = result.Payload;

        // Assert
        Assert.Equal(eventId, invitation.ViaEventId);
        Assert.Equal(guestId, invitation.ViaGuestId);
    }
    [Fact]
    public void Create_ShouldHavePendingStatusAfterCreate()
    {
        // Arrange
        var invitationId = ViaInvitationId.Create().Payload;
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;

        // Act
        var result = ViaInvitation.Create(invitationId, eventId, guestId);
        var invitation = result.Payload;

        // Assert
        Assert.Equal(ViaInvitationStatus.Pending, invitation.Status);
    }
    
}

