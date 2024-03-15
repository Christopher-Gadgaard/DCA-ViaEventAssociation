using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Guest.Register;

public class CreateGuestTest
{

    [Fact]
    public void Create_WithValidInputs_ShouldSuccessfullyCreateGuest()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload; // Assuming Create() method exists and returns a valid ID.
        var guestFirstName = "John";
        var guestLastName = "Doe";
        var email = "john@via.dk";

        var viaGuestNameResult = ViaGuestName.Create(guestFirstName, guestLastName);
        Assert.True(viaGuestNameResult.IsSuccess, "Precondition: valid guest name");

        var viaEmailResult = ViaEmail.Create(email);
        Assert.True(viaEmailResult.IsSuccess, "Precondition: valid email");

        // Act
        var viaGuest = ViaGuest.Create(guestId, viaGuestNameResult.Payload, viaEmailResult.Payload);

        // Assert
        Assert.NotNull(viaGuest);
        Assert.Equal(guestId, viaGuest.Payload.Id);
        Assert.Equal(viaGuestNameResult.Payload, viaGuest.Payload.ViaGuestName);
        Assert.Equal(viaEmailResult.Payload, viaGuest.Payload.ViaEmail);
    }


}