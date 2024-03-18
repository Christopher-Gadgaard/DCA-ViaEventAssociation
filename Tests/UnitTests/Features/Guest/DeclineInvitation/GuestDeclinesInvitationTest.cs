using Moq;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Aggregates.Locations;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using Via.EventAssociation.Core.Domain.Services;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.DeclineInvitation;

public class GuestDeclinesInvitationTest
{
    [Fact]
    public void Guest_Declines_Invitation_For_Active_Event_Invitation_Registered_As_Declined()
    {
        // Arrange
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        var email = "john@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);

        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var invitationId = ViaInvitationId.Create().Payload;
        var guest = ViaGuest.Create(
            guestId,
            ViaGuestName.Create("John", "Doe").Payload,
            ViaEmail.Create("john@via.dk", emailCheckerMock.Object).Payload).Payload;
        // Setting up an active event and a pending invitation
        var viaEvent = ViaEvent.Create(eventId).Payload;
        var invitation = ViaInvitation.Create(invitationId, eventId, guestId).Payload;

        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(invitation);
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>())).Returns(OperationResult<ViaInvitation>.Success(invitation));
        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockEventRepository.Setup(repo => repo.Update(It.IsAny<ViaEvent>())).Returns(OperationResult<ViaEvent>.Success(viaEvent));
        mockGuestRepository.Setup(repo => repo.GetById(guestId)).Returns(guest);
        mockGuestRepository.Setup(repo => repo.Update(It.IsAny<ViaGuest>())).Returns(OperationResult<ViaGuest>.Success(guest));

        var invitationService = new GuestDeclinesInvitation(mockInvitationRepository.Object,mockEventRepository.Object, mockGuestRepository.Object);

        // Act
        var result = invitationService.DeclineInvitation(invitationId);

        // Assert
        Assert.True(result.IsSuccess);
        mockInvitationRepository.Verify(repo => repo.Update(It.Is<ViaInvitation>(i => i.Status == ViaInvitationStatus.Rejected)), Times.Once);
        Assert.Equal(ViaInvitationStatus.Rejected, invitation.Status);
    }
    [Fact]
    public void Guest_Declines_Already_Accepted_Invitation_Invitation_Registered_As_Declined()
    {
        // Arrange
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        var email = "john@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var guestId = ViaGuestId.Create().Payload;
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var guest = ViaGuest.Create(
            guestId,
            ViaGuestName.Create("John", "Doe").Payload,
            ViaEmail.Create("john@via.dk", emailCheckerMock.Object).Payload).Payload;
        var eventId = ViaEventId.Create().Payload;
        var invitationId = ViaInvitationId.Create().Payload;

        // Setting up an active event and an accepted invitation
        var viaEvent = ViaEvent.Create(eventId).Payload;
        var acceptedInvitation = ViaInvitation.Create(invitationId, eventId, guestId).Payload;
        var maxGuests = ViaMaxGuests.Create(5).Payload;
        var title = ViaEventTitle.Create("Event Title").Payload;
        var description = ViaEventDescription.Create("Event Description").Payload;
        var locationId = ViaLocationId.Create().Payload;
        var locationName = ViaLocationName.Create("Location Name").Payload;
        var locationCapacity = ViaLocationCapacity.Create(20).Payload;
        
        var location = ViaLocation.Create(locationId,locationName,locationCapacity).Payload;
        viaEvent.SetMaxGuests(maxGuests);
        viaEvent.UpdateTitle(title);
        viaEvent.UpdateDescription(description);
        viaEvent.MakePublic();
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(OperationResult<ViaInvitation>.Success(acceptedInvitation));
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>()))
            .Returns((ViaInvitation inv) => OperationResult<ViaInvitation>.Success(inv));
        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockGuestRepository.Setup(repo => repo.GetById(guestId)).Returns(guest);
var invitationAccepted = new GuestAcceptsInvitation( mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);
invitationAccepted.AcceptInvitation( invitationId);
Assert.Equal(ViaInvitationStatus.Accepted, acceptedInvitation.Status);

        var invitationService = new GuestDeclinesInvitation(mockInvitationRepository.Object, mockEventRepository.Object, mockGuestRepository.Object);

        // Act
        var result = invitationService.DeclineInvitation(invitationId);

        // Assert
        Assert.True(result.IsSuccess);
        mockInvitationRepository.Verify(repo => repo.Update(It.Is<ViaInvitation>(i => i.Status == ViaInvitationStatus.Rejected)));
        Assert.Equal(ViaInvitationStatus.Rejected, acceptedInvitation.Status);
    }
   
    [Fact]
    public void Guest_Accepts_Nonexistent_Invitation_Fails_With_Error_Message()
    {
        // Arrange
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var invitationId = ViaInvitationId.Create().Payload;

        // Simulating no invitation exists for the given guest to the event
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId))
            .Returns(OperationResult<ViaInvitation>.Failure(new List<OperationError>(){ new OperationError(ErrorCode.NotFound, "Invitation not found")}));

        var invitationService = new GuestAcceptsInvitation(mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);

        // Act
        var result = invitationService.AcceptInvitation(invitationId);

        // Assert
        Assert.False(result.IsSuccess);
       
    }
    
    
}