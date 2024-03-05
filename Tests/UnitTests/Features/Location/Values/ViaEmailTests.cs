using Via.EventAssociation.Core.Domain.Aggregates.Guests;

namespace UnitTests.Features.Location.Values;

public class ViaEmailTests
{
    [Fact]

    public void Create_ShouldReturnSuccess()
    {
        // Arrange
        var validEmail = "valid_mail@via.dk";

        // Act
        var result = ViaEmail.Create(validEmail);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(validEmail, result.Payload.Value);
    }

    [Fact]

    public void Create_ShouldReturnFailureWhenEmpty()
    {
        // Arrange
        var emptyEmail = ViaEmail.Create("");

        // Act
        var result = emptyEmail;

        // Assert
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]

    public void Create_ShouldReturnFailureWhenInvalid()
    {
        // Arrange
        var invalidEmail = ViaEmail.Create("invalid_mail");

        // Act
        var result = invalidEmail;

        // Assert
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]

    public void Create_ShouldReturnFailureWhenNull()
    {
        // Arrange
        var nullEmail = ViaEmail.Create(null);

        // Act
        var result = nullEmail;

        // Assert
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]

    public void Create_ShouldReturnFailureWhenTooLong()
    {
        // Arrange
        var tooLongEmail = ViaEmail.Create("waytoolongemailaddressplaceholderstuffforsize@via.dk");

        // Act
        var result = tooLongEmail;

        // Assert
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]

    public void Create_ShouldReturnFailureWhenTooShort()
    {
        // Arrange
        var tooShortEmail = ViaEmail.Create("a@b");

        // Act
        var result = tooShortEmail;

        // Assert
        Assert.True(result.OperationErrors.Any());
    }

    [Fact]

    public void Create_ShouldReturnFailureWhenInvalidDomain()
    {
        // Arrange
        var invalidDomainEmail = ViaEmail.Create("vlad@vea.dk");

        // Act
        var result = invalidDomainEmail;

        // Assert

        Assert.True(result.OperationErrors.Any());
    }

    [Fact]

    public void ShouldReturnSuccess_WhenEmailIsValid()
    {
        // Arrange
        const string validEmail = "308826@via.dk";

        // Act
        var result = ViaEmail.Create(validEmail);

        // Assert
        Assert.True(result.IsSuccess);

    }

    [Fact]

    public void ShouldReturnFailure_WhenEmailIsInvalid()
    {
        // Arrange
        const string invalidEmail = "AB1234@via.dk";

        // Act
        var result = ViaEmail.Create(invalidEmail);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]

    public void ShouldReturnFailure_WhenEmailIsInvalid2()
    {
        // Arrange
        const string invalidEmail = "308826@via.md";

        // Act
        var result = ViaEmail.Create(invalidEmail);

        // Assert
        Assert.False(result.OperationErrors.Any());
    }
}