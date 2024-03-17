using Moq;
using Xunit;
using System.Linq;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Location.Values;

public class ViaEmailTests
{
    [Fact]
    public void Create_ShouldReturnFailureWhenEmailIsAlreadyRegistered()
    {
        var validEmail = "mail@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(validEmail)).Returns(false);
        var result = ViaEmail.Create(validEmail, emailCheckerMock.Object);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_ShouldReturnFailureWhenEmpty()
    {
        var email = "";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var result = ViaEmail.Create(email, emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void Create_ShouldReturnFailureWhenInvalid()
    {
        var email = "invalid_mail";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var result = ViaEmail.Create(email, emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void Create_ShouldReturnFailureWhenNull()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(null)).Returns(false);
        var result = ViaEmail.Create(null, emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void Create_ShouldReturnFailureWhenTooLong()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("waytoolongemailaddressplaceholderstuffforsize@via.dk")).Returns(false);
        var result = ViaEmail.Create("waytoolongemailaddressplaceholderstuffforsize@via.dk", emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void Create_ShouldReturnFailureWhenTooShort()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("a@b")).Returns(false);
        var result = ViaEmail.Create("a@b", emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void Create_ShouldReturnFailureWhenInvalidDomain()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("vlad@vea.dk")).Returns(false);
        var result = ViaEmail.Create("vlad@vea.dk", emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void ShouldReturnSuccess_WhenEmailIsValid()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("308826@via.dk")).Returns(false);
        var result = ViaEmail.Create("308826@via.dk", emailCheckerMock.Object);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ShouldReturnFailure_WhenEmailIsInvalid()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("AB1234@via.dk")).Returns(false);
        var result = ViaEmail.Create("AB1234@via.dk", emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void ShouldReturnFailure_WhenEmailIsInvalid2()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("308826@via.md")).Returns(false);
        var result = ViaEmail.Create("308826@via.md", emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void ShouldReturnFailure_WhenEmailIsInvalid3()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("308826@VIA.DK")).Returns(false);
        var result = ViaEmail.Create("308826@VIA.DK", emailCheckerMock.Object);
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]
    public void ShouldReturnSuccess_OnCapitalLetters()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("VLAD@via.dk")).Returns(false);
        var result = ViaEmail.Create("VLAD@via.dk", emailCheckerMock.Object);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ShouldFail_WhenEmailIsInvalid()
    {
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered("john@gmail.com")).Returns(false);
        var result = ViaEmail.Create("john@gmail.com", emailCheckerMock.Object);
        Assert.False(result.IsSuccess);
    }
}
