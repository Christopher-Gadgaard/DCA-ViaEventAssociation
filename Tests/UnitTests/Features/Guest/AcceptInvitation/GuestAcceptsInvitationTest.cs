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

namespace UnitTests.Features.Guest.AcceptInvitation;

public class GuestAcceptsInvitationTest
{
    [Fact]
    public void Guest_Accepts_Pending_Invitation_To_Active_Event_Successfully()
    {
        // Arrange
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var invitationId = ViaInvitationId.Create().Payload;

        // Assuming a setup where the event is active and below max guest capacity
      
     
        var email = "john@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var viaEvent = ViaEvent.Create(
            eventId
        ).Payload;
        var guest = ViaGuest.Create(
            guestId,
            ViaGuestName.Create("John", "Doe").Payload,
            ViaEmail.Create("john@via.dk", emailCheckerMock.Object).Payload).Payload;
        // Act
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
        // Creating a pending invitation for the guest
        var invitation = ViaInvitation.Create(invitationId, eventId, guestId).Payload;

        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(invitation);
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>())).Returns(OperationResult<ViaInvitation>.Success(invitation));
        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockEventRepository.Setup(repo => repo.Update(It.IsAny<ViaEvent>())).Returns(OperationResult<ViaEvent>.Success(viaEvent));
        mockGuestRepository.Setup(repo => repo.GetById(guestId)).Returns(guest);
        mockGuestRepository.Setup(repo => repo.Update(It.IsAny<ViaGuest>())).Returns(OperationResult<ViaGuest>.Success(guest));
        var invitationService =new GuestAcceptsInvitation( mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);

        // Act
        var result = invitationService.AcceptInvitation(invitationId);

        // Assert
        Assert.True(result.IsSuccess);
        mockInvitationRepository.Verify(repo => repo.Update(It.Is<ViaInvitation>(i => i.Status == ViaInvitationStatus.Accepted)), Times.Once);
        Assert.Equal(ViaInvitationStatus.Accepted, invitation.Status);
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

        mockInvitationRepository.Setup(repo => repo.GetById(invitationId))
            .Returns(OperationResult<ViaInvitation>.Failure(new List<OperationError>(){ new OperationError(ErrorCode.NotFound, "Invitation not found")}));

        var invitationService = new GuestAcceptsInvitation(mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);

        // Act
        var result = invitationService.AcceptInvitation(invitationId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.NotFound && error.Message == "Invitation not found");
    }
    
    [Fact]
    public void Guest_Accepts_Invitation_For_Full_Event_Fails_With_Error_Message()
    {
        // Arrange
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var invitationId = ViaInvitationId.Create().Payload;
        var email = "john@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var viaEvent = ViaEvent.Create(
            eventId
        ).Payload;
        var guest = ViaGuest.Create(
            guestId,
            ViaGuestName.Create("John", "Doe").Payload,
            ViaEmail.Create("john@via.dk", emailCheckerMock.Object).Payload).Payload;
        // Act
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
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        // Simulating the event being full
        for (int i = 0; i < maxGuests.Value; i++)
        {
            viaEvent.AddParticipant(ViaGuestId.Create().Payload);
        }

        var pendingInvitation = ViaInvitation.Create(invitationId, eventId, guestId);
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId))
            .Returns(OperationResult<ViaInvitation>.Success(pendingInvitation.Payload));
        mockEventRepository.Setup(repo => repo.GetById(eventId))
            .Returns(viaEvent);
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(pendingInvitation);
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>())).Returns(OperationResult<ViaInvitation>.Failure(new List<OperationError>(){ new OperationError(ErrorCode.Conflict, "The event is full")}));
        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockEventRepository.Setup(repo => repo.Update(It.IsAny<ViaEvent>())).Returns(OperationResult<ViaEvent>.Success(viaEvent));
        mockGuestRepository.Setup(repo => repo.GetById(guestId)).Returns(guest);
        mockGuestRepository.Setup(repo => repo.Update(It.IsAny<ViaGuest>())).Returns(OperationResult<ViaGuest>.Success(guest));
        var invitationService = new GuestAcceptsInvitation(mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);

        // Act
        var result = invitationService.AcceptInvitation(invitationId);

        // Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]
    public void Guest_Accepts_Invitation_For_Cancelled_Event_Fails_With_Error_Message()
    {
        // Arrange
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var invitationId = ViaInvitationId.Create().Payload;
        var email = "john@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);

        var guest = ViaGuest.Create(
            guestId,
            ViaGuestName.Create("John", "Doe").Payload,
            ViaEmail.Create(email, emailCheckerMock.Object).Payload).Payload;
        // Setting up a cancelled event
        var viaEvent = ViaEvent.Create(eventId).Payload;
        viaEvent.UpdateStatus(ViaEventStatus.Cancelled);

        var pendingInvitation = ViaInvitation.Create(invitationId, eventId, guestId).Payload;

        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(OperationResult<ViaEvent>.Success(viaEvent));
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(OperationResult<ViaInvitation>.Success(pendingInvitation));
        
        // Assuming the Update method on the repository should reflect the new status, we simulate that as well.
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>()))
            .Returns((ViaInvitation inv) => OperationResult<ViaInvitation>.Success(inv));

        var invitationService = new GuestAcceptsInvitation(mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);

        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(pendingInvitation);
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>())).Returns(OperationResult<ViaInvitation>.Failure(new List<OperationError>(){ new OperationError(ErrorCode.Conflict, "The event is full")}));
        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockEventRepository.Setup(repo => repo.Update(It.IsAny<ViaEvent>())).Returns(OperationResult<ViaEvent>.Failure(new List<OperationError>(){ new OperationError(ErrorCode.Conflict, "The event is full")}));
        mockGuestRepository.Setup(repo => repo.GetById(guestId)).Returns(guest);
        mockGuestRepository.Setup(repo => repo.Update(It.IsAny<ViaGuest>())).Returns(OperationResult<ViaGuest>.Success(guest));
        // Act
        var result = invitationService.AcceptInvitation(invitationId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.Conflict );
    }
    [Fact]
    public void Guest_Accepts_Invitation_For_Ready_Event_Fails_With_Error_Message()
    {
        // Arrange
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var mockGuestRepository = new Mock<IViaGuestRepository>();

        // Setting up a ready event
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var invitationId = ViaInvitationId.Create().Payload;

   
        var email = "john@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var viaEvent = ViaEvent.Create(
            eventId
        ).Payload;
        var guest = ViaGuest.Create(
            guestId,
            ViaGuestName.Create("John", "Doe").Payload,
            ViaEmail.Create("john@via.dk", emailCheckerMock.Object).Payload).Payload;
        // Act
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
        viaEvent.UpdateStatus(ViaEventStatus.Ready);

        var pendingInvitation = ViaInvitation.Create(invitationId, eventId, guestId).Payload;
        Assert.Equal(ViaInvitationStatus.Pending, pendingInvitation.Status);

        // Assuming the invitation's status update to reflect its new state
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>()))
            .Returns((ViaInvitation inv) => OperationResult<ViaInvitation>.Success(inv));
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(pendingInvitation);
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>())).Returns(OperationResult<ViaInvitation>.Success(pendingInvitation));
        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockEventRepository.Setup(repo => repo.Update(It.IsAny<ViaEvent>())).Returns(OperationResult<ViaEvent>.Failure(new List<OperationError>(){ new OperationError(ErrorCode.Conflict, "The event cannot yet be joined")}));
        mockGuestRepository.Setup(repo => repo.GetById(guestId)).Returns(guest);
        mockGuestRepository.Setup(repo => repo.Update(It.IsAny<ViaGuest>())).Returns(OperationResult<ViaGuest>.Success(guest));
        // Act
        var invitationService = new GuestAcceptsInvitation(mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);
        
        var result = invitationService.AcceptInvitation(invitationId);

        // Assert
        Assert.Equal(viaEvent.Status, ViaEventStatus.Ready);
        Assert.False(result.IsSuccess);
    }
}