using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Guest.Register;

public class CreateGuestTest
{
    [Fact]
    public void Create_WithValidInputs_ShouldSuccessfullyCreateGuest()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload; 
        var guestName = ViaGuestName.Create("John", "Doe"); 
        var email = ViaEmail.Create("john@via.dk"); 
        // Act
        var result = ViaGuest.Create(guestId, guestName.Payload, email.Payload);

        // Assert
        Assert.True(result.IsSuccess);
        var viaGuest = result.Payload;
        Assert.NotNull(viaGuest);
        Assert.Equal(guestId, viaGuest.Id);
        Assert.Equal(guestName.Payload, viaGuest.ViaGuestName);
        Assert.Equal(email.Payload, viaGuest.ViaEmail);
    }


    [Fact]
    public void Create_WithEmailNotEndingViaDk_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("John", "Doe");
        var emailResult = ViaEmail.Create("john@gmail.com"); 

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
        // Assert specific errors if necessary
    }

    [Fact]
    public void Create_WithEmailInIncorrectFormat_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("John", "Doe");
        var emailResult = ViaEmail.Create("invalid_email_format"); 

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]
    public void Create_WithInvalidFirstName_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("J", "Doe"); 
        var emailResult = ViaEmail.Create("john@via.dk");

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]
    public void Create_WithInvalidLastName_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("John", "D"); 
        var emailResult = ViaEmail.Create("john@via.dk");

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]
    public void Create_WithFirstNameContainingNumbers_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("John3", "Doe");
        var emailResult = ViaEmail.Create("john@via.dk");

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Create_WithLastNameContainingNumbers_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("John", "Doe3");
        var emailResult = ViaEmail.Create("john@via.dk");

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
    }
    [Fact]
    public void Create_WithFirstNameContainingSymbols_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("John!", "Doe");
        var emailResult = ViaEmail.Create("john@via.dk");

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Create_WithLastNameContainingSymbols_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("John", "Doe!");
        var emailResult = ViaEmail.Create("john@via.dk");

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
    }


}