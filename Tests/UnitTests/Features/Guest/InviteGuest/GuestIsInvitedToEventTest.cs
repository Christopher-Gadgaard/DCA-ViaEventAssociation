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

namespace UnitTests.Features.Guest.InviteGuest;

public class GuestIsInvitedToEventTest
{
    [Fact]
    public void Guest_Receives_Invitation_To_Ready_And_Not_Full_Event_Successfully()
    {
        // Arrange
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
    
        var invitationId = ViaInvitationId.Create().Payload;
        var eventId = ViaEventId.Create().Payload;
        var guestId =  ViaGuestId.Create().Payload;
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
        viaEvent.MakePrivate();
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        
        var invitation = ViaInvitation.Create(invitationId, eventId, guestId).Payload;
        
        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockGuestRepository.Setup(repo => repo.GetById(guestId)).Returns(guest); 
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(invitation);
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>()))
            .Returns((ViaInvitation viaInvitation) => viaInvitation);
        var sendInvitationService = new SendInvitation(mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);

        // Act
        var result = sendInvitationService.SendInvitationToEvent(invitationId);

        // Assert
        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(result.OperationErrors, new List<OperationError>());
        Assert.True(result.IsSuccess);
    }
    
    [Theory]
    [InlineData(ViaEventStatus.Draft)]
    [InlineData(ViaEventStatus.Cancelled)]
    public void Invitation_Fails_For_Non_Ready_Or_Active_Event(ViaEventStatus eventStatus)
    {
        // Arrange
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var invitationId = ViaInvitationId.Create().Payload;
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;

        var viaEvent = ViaEvent.Create(eventId).Payload;

        var invitation = ViaInvitation.Create(invitationId, eventId, guestId).Payload;

        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(invitation);

        var sendInvitationService = new SendInvitation(mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);

        // Act
        var result = sendInvitationService.SendInvitationToEvent(invitationId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.Conflict && error.Message == "Cant send invitation. The event is not in a ready or active state.");
    }
    
    [Fact]
    public void Invitation_Fails_For_Full_Event()
    {
        // Arrange
        var mockGuestRepository = new Mock<IViaGuestRepository>();
        var mockInvitationRepository = new Mock<IViaInvitationRepository>();
        var mockEventRepository = new Mock<IViaEventRepository>();
        var invitationId = ViaInvitationId.Create().Payload;
  

        var eventId = ViaEventId.Create().Payload;
        var guestId =  ViaGuestId.Create().Payload;
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
        
        for (int i = 0; i < maxGuests.Value; i++)
        {
           viaEvent.AddParticipant(ViaGuestId.Create().Payload);
        }

        var invitation = ViaInvitation.Create(invitationId, eventId, guestId).Payload;

        mockEventRepository.Setup(repo => repo.GetById(eventId)).Returns(viaEvent);
        mockGuestRepository.Setup(repo => repo.GetById(guestId)).Returns(guest); 
        mockInvitationRepository.Setup(repo => repo.GetById(invitationId)).Returns(invitation);
        mockInvitationRepository.Setup(repo => repo.Update(It.IsAny<ViaInvitation>()))
            .Returns((ViaInvitation viaInvitation) => viaInvitation);
        var sendInvitationService = new SendInvitation(mockGuestRepository.Object, mockInvitationRepository.Object, mockEventRepository.Object);

        // Act
        var result = sendInvitationService.SendInvitationToEvent(invitationId);

        // Assert
        Assert.Equal(true, viaEvent.IsFull());
        Assert.False(result.IsSuccess);
        
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.Conflict && error.Message == "The event is full.");
    }
    
}