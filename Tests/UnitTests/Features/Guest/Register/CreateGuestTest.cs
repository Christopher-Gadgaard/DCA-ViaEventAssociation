using Moq;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;

namespace UnitTests.Features.Guest.Register;

public class CreateGuestTest
{
    [Fact]
    public void Create_WithValidInputs_ShouldSuccessfullyCreateGuest()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload; 
        var guestName = ViaGuestName.Create("John", "Doe"); 
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "john@via.dk";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);
        // Act
        var result = ViaGuest.Create(guestId, guestName.Payload, emailResult);

        // Assert
        Assert.True(result.IsSuccess);
        var viaGuest = result.Payload;
        Assert.NotNull(viaGuest);
        Assert.Equal(guestId, viaGuest.Id);
        Assert.Equal(guestName.Payload, viaGuest.ViaGuestName);
        Assert.Equal(emailResult.Payload, viaGuest.ViaEmail);
    }


    [Fact]
    public void Create_WithEmailNotEndingViaDk_ShouldFail()
    {
        // Arrange
        var guestId = ViaGuestId.Create().Payload;
        var guestNameResult = ViaGuestName.Create("John", "Doe");
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "john@gmail.com";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);


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
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "invalidemail";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);

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
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "john@via.dk";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);

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
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "john@via.dk";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);

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
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "john@via.dk";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);

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
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "john@via.dk";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);

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
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "john@via.dk";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);

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
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        var email = "john@via.dk";
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var emailResult = ViaEmail.Create(email, emailCheckerMock.Object);

        // Act
        var result = ViaGuest.Create(guestId, guestNameResult, emailResult);

        // Assert
        Assert.False(result.IsSuccess);
    }

    public class ViaGuestCreationTests
    {
        [Fact]
        public void Create_WithEmailAlreadyRegistered_ShouldFail()
        {
            // Arrange
             var email = "alreadyregistered@via.dk";
            var emailCheckerMock = new Mock<ICheckEmailInUse>();
            emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(true);
            
            // Act
            var emailCopy = ViaEmail.Create(email, emailCheckerMock.Object);
            var result = ViaGuest.Create(ViaGuestId.Create().Payload, ViaGuestName.Create("John", "Doe").Payload,
                emailCopy);


            // Assert
            Assert.False(result.IsSuccess);
        }
    }
}