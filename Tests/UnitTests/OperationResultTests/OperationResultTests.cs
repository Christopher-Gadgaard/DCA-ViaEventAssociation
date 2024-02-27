using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.OperationResultTests;

public class OperationResultTests
{
    [Fact]
    public void OperationError_WithErrorCode_SetsMessageFromDescription()
    {
        // Arrange
        var errorCode = ErrorCode.NotFound;

        // Act
        var operationError = new OperationError(errorCode);

        // Assert
        Assert.Equal(errorCode.GetDescription(), operationError.Message);
    }

    [Fact]
    public void OperationError_WithSpecificMessage_OverridesDefaultMessage()
    {
        // Arrange
        var specificMessage = "Custom error message";

        // Act
        var operationError = new OperationError(ErrorCode.BadRequest, specificMessage);

        // Assert
        Assert.Equal(specificMessage, operationError.Message);
    }

    [Theory]
    [InlineData(ErrorCode.InvalidInput, "Input data cannot be null or empty.")]
    [InlineData(ErrorCode.NotFound, "The requested resource was not found.")]
    public void ErrorCodeExtensions_GetDescription_ReturnsExpectedDescription(ErrorCode errorCode, string expectedDescription)
    {
        // Act
        var description = errorCode.GetDescription();

        // Assert
        Assert.Equal(expectedDescription, description);
    }

    [Fact]
    public void OperationResult_SuccessWithPayload_SetsIsSuccessTrueAndAssignsPayload()
    {
        // Arrange
        var payload = "Test Payload";

        // Act
        var result = OperationResult<string>.SuccessWithPayload(payload);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(payload, result.Payload);
    }

    [Fact]
    public void OperationResult_Failure_RecordsErrorsAndSetsIsSuccessFalse()
    {
        // Arrange
        var operationErrors = new List<OperationError> { new OperationError(ErrorCode.InternalServerError) };

        // Act
        var result = OperationResult<string>.Failure(operationErrors);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.OperationErrors);
    }

    [Fact]
    public void OperationError_WithNullSpecificMessage_UsesErrorCodeDescription()
    {
        // Arrange
        var errorCode = ErrorCode.UnauthorizedAccess;

        // Act
        var operationError = new OperationError(errorCode, null);

        // Assert
        Assert.Equal(errorCode.GetDescription(), operationError.Message);
    }

    [Fact]
    public void OperationError_WithType_SetsTypeProperty()
    {
        // Arrange
        var type = "ValidationError";

        // Act
        var operationError = new OperationError(ErrorCode.BadRequest, "Bad request", type);

        // Assert
        Assert.Equal(type, operationError.Type);
    }
    
    [Fact]
    public void GetDescription_WithoutDescriptionAttribute_ReturnsEnumName()
    {
        // Arrange
        var errorCode = ErrorCode.Undefined; 

        // Act
        var description = errorCode.GetDescription();

        // Assert
        Assert.Equal("Undefined", description);
    }

    [Fact]
    public void SuccessWithoutPayload_ReturnsSuccessWithDefaultPayload()
    {
        // Act
        var result = OperationResult<string>.SuccessWithoutPayload();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Payload); 
    }

    [Fact]
    public void ImplicitConversion_FromPayload_CreatesSuccessResult()
    {
        // Arrange
        var payload = "TestData";

        // Act
        OperationResult<string> result = payload; 

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(payload, result.Payload);
    }
}