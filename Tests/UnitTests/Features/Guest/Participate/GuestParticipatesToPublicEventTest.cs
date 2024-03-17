using Moq;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Aggregates.Locations;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Guest.Participate;

public class GuestParticipatesToPublicEventTest
{
    [Fact]
    public void Guest_Participates_In_Public_Event_Successfully()
    {
        // Arrange
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
        // Act
        var maxGuests = ViaMaxGuests.Create(10).Payload;
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
        var result = viaEvent.AddParticipant(guestId);
    
        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(viaEvent.IsParticipant(guestId));
    } 
    
    [Fact]
    public void Guest_Participation_Fails_When_Event_Is_In_Draft_State()
    {
        // Arrange
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var email = "example@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);

        var viaEvent = ViaEvent.Create(eventId).Payload;
   
        var maxGuests = ViaMaxGuests.Create(10).Payload;
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
            
        var guest = ViaGuest.Create(
            guestId,
            ViaGuestName.Create("John", "Doe").Payload,
            ViaEmail.Create(email, emailCheckerMock.Object).Payload).Payload;
        

        // Act
        var result = viaEvent.AddParticipant(guestId);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Guest_Participation_Fails_When_Event_Is_Private()
    {
        // Arrange
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var email = "john@via.dk";
        
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("example@via.dk")).Returns(false);

        var guest = ViaGuest.Create(
            guestId,
            ViaGuestName.Create("John", "Doe").Payload,
            ViaEmail.Create(email, emailCheckerMock.Object).Payload).Payload;
        
        var viaEvent = ViaEvent.Create(eventId).Payload;
        
        
        var maxGuests = ViaMaxGuests.Create(10).Payload;
        var title = ViaEventTitle.Create("Event Title").Payload;
        var description = ViaEventDescription.Create("Event Description").Payload;
        var locationId = ViaLocationId.Create().Payload;
        var locationName = ViaLocationName.Create("Location Name").Payload;
        var locationCapacity = ViaLocationCapacity.Create(20).Payload;
        var location = ViaLocation.Create(locationId,locationName,locationCapacity).Payload;
        viaEvent.SetMaxGuests(maxGuests);
        viaEvent.UpdateTitle(title);
        viaEvent.UpdateDescription(description);
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        
        viaEvent.MakePrivate(); 

        // Act
        var result = viaEvent.AddParticipant(guestId);

        // Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]
    public void Guest_Participation_Fails_When_Already_A_Participant()
    {
        // Arrange
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("participant@via.dk")).Returns(false);

        var viaEvent = ViaEvent.Create(eventId).Payload;
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        viaEvent.MakePublic();

        viaEvent.AddParticipant(guestId);

        // Act
        var result = viaEvent.AddParticipant(guestId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.BadRequest);
    }
    [Fact]
    public void Guest_Participation_Fails_When_Event_Is_Canceled()
    {
        // Arrange
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("canceled@via.dk")).Returns(false);
        var viaEvent = ViaEvent.Create(eventId).Payload;
        viaEvent.UpdateStatus(ViaEventStatus.Cancelled);

        // Act
        var result = viaEvent.AddParticipant(guestId);

        // Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]
    public void Guest_Participation_Fails_When_Event_Is_Full()
    {
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


        for (int i = 0; i < maxGuests.Value; i++)
        {
                   var tempGuestId = ViaGuestId.Create().Payload; 
                        viaEvent.AddParticipant(tempGuestId);
        }
     
        

        // Act
        var result = viaEvent.AddParticipant(guestId);

        // Assert
       Assert.False(result.IsSuccess);
       Assert.Equal(viaEvent.Status, ViaEventStatus.Active);
       Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.Conflict && error.Message == "The event is full.");
    }
    
    [Fact]
    public void Guest_Participation_Fails_When_Event_Start_Time_Is_In_The_Past()
    {
        // Arrange
        var startTime = new DateTime(2022, 08, 25, 10, 00, 00);
        var endTime = startTime.AddDays(1);
        var dateTimeRange = ViaDateTimeRange.Create(startTime, endTime);
        Assert.False(dateTimeRange.IsSuccess);
        
        
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
        

        viaEvent.UpdateDateTimeRange(dateTimeRange.Payload);
        
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        
        var result = viaEvent.AddParticipant(guestId);
        //should default to the current time and success
       Assert.True(result.IsSuccess);
        
    
    }
    
}