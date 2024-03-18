using Moq;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Aggregates.Locations;
using Via.EventAssociation.Core.Domain.Common.Utilities.Interfaces;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Guest.CancelParticipation;

public class GuestCancelsEventParticipationTest
{
    [Fact]
    public void Guest_Removes_Participation_From_Public_Event_Successfully()
    {
        // Arrange

        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var startTime = new DateTime(2024, 08, 25, 10, 00, 00);
        var endTime = startTime.AddHours(2);
        var dateTimeRange = ViaDateTimeRange.Create(startTime, endTime).Payload;
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
        
        var location = ViaLocation.Create(locationId, locationName, locationCapacity).Payload;
        viaEvent.SetMaxGuests(maxGuests);
        viaEvent.UpdateTitle(title);
        viaEvent.UpdateDescription(description);
        viaEvent.UpdateDateTimeRange(dateTimeRange);
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        viaEvent.MakePublic();
        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        viaEvent.AddParticipant(guestId);
        Assert.True(viaEvent.IsParticipant(guestId));
        // Act
        var result = viaEvent.RemoveParticipant(guestId);
        // Assert
        Assert.Equal(result.OperationErrors, new List<OperationError>());
        Assert.True(result.IsSuccess);
        Assert.False(viaEvent.IsParticipant(guestId));
    }

    [Fact]
    public void Guest_Removal_Does_Nothing_When_Not_A_Participant()
    {
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
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

        var location = ViaLocation.Create(locationId, locationName, locationCapacity).Payload;
        viaEvent.SetMaxGuests(maxGuests);
        viaEvent.UpdateTitle(title);
        viaEvent.UpdateDescription(description);
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        viaEvent.UpdateStatus(ViaEventStatus.Active);
        viaEvent.MakePublic();

        // Act
        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        var result = viaEvent.RemoveParticipant(guestId);

        // Assert
        Assert.Contains(result.OperationErrors,
            error => error.Code == ErrorCode.NotFound);
        Assert.False(result.IsSuccess);
        Assert.False(viaEvent.IsParticipant(guestId));
    }

    [Fact]
    public void Guest_Removal_Fails_For_Ongoing_Event()
    {
        // Arrange
        var eventId = ViaEventId.Create().Payload;
        var guestId = ViaGuestId.Create().Payload;
        var email = "john@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();

        // Setting the event's start time to be in the past, making it "ongoing"
        var startTime = new DateTime(2022, 08, 25, 10, 00, 00);
        var endTime = startTime.AddDays(1);
        var dateTimeRange = ViaDateTimeRange.Create(startTime, endTime).Payload;

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

        var location = ViaLocation.Create(locationId, locationName, locationCapacity).Payload;
        viaEvent.SetMaxGuests(maxGuests);
        viaEvent.UpdateTitle(title);
        viaEvent.UpdateDescription(description);
        viaEvent.MakePublic();

        var time = viaEvent.UpdateDateTimeRange(dateTimeRange);
        Assert.Equal(time.OperationErrors, new List<OperationError>());
        var addParticipant = viaEvent.AddParticipant(guestId);
        Assert.True(addParticipant.IsFailure);
    }
}