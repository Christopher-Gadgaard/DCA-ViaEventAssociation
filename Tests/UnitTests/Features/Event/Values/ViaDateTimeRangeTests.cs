using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.Values;

public class ViaDateTimeRangeTests
{
    [Fact]
    public void Create_ValidDateTimeRange_ReturnsSuccess()
    {
        // Arrange
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateValidDateRange();

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(start, result.Payload.StartValue);
        Assert.Equal(end, result.Payload.EndValue);
    }

    [Fact]
    public void Create_ValidDateRangeWithStartTimeAtBoundary_ReturnsSuccess()
    {
        // Arrange
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateValidDateRange_StartTimeAtBoundary();

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(start, result.Payload.StartValue);
        Assert.Equal(end, result.Payload.EndValue);
    }

    [Fact]
    public void Create_InvalidDateRangeWithStartTimeBeforeBoundary_ReturnsFailure()
    {
        // Arrange
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateInvalidDateRange_StartTimeBeforeBoundary();

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors,
            error => error is {Message: not null, Code: ErrorCode.BadRequest} &&
                     error.Message.Contains("Events cannot start before 08:00 AM."));
    }

    [Fact]
    public void Create_ValidDateRangeWithEndTimeAtBoundary_ReturnsSuccess()
    {
        // Arrange
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateValidDateRange_EndTimeAtNextDayBoundary();

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(start, result.Payload.StartValue);
        Assert.Equal(end, result.Payload.EndValue);
    }

    [Fact]
    public void Create_InvalidDateRangeWithEndTimeAfterBoundary_ReturnsFailure()
    {
        // Arrange
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateInvalidDateRange_EndTimeAfterNextDayBoundary();

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors,
            error => error is {Message: not null, Code: ErrorCode.BadRequest} &&
                     error.Message.Contains("If the event spans to the next day, it must end by 01:00 AM."));
    }

    [Fact]
    public void Create_StartAfterEndDateTime_ReturnsFailure()
    {
        // Arrange
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateInvalidDateRange_StartAfterEnd();

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.BadRequest);
    }

    [Fact]
    public void Create_InvalidStartTime_ReturnsFailure()
    {
        // Arrange
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateInvalidDateRange_InvalidStartTime();

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors,
            error => error is {Message: not null, Code: ErrorCode.BadRequest} &&
                     error.Message.Contains("Events cannot start before 08:00 AM."));
    }

    [Fact]
    public void Create_EventInvalidDuration_ReturnsFailure()
    {
        // Arrange
        var (start, end) = ViaDateTimeRangeTestDataFactory.CreateInvalidDateRange_InvalidDuration();

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors,
            error => error is {Message: not null, Code: ErrorCode.BadRequest} &&
                     error.Message.Contains("The event duration must be at least 1 hour and no more than 10 hours."));
    }

    [Fact]
    public void Create_MultipleValidationFailures_ReturnsAllFailures()
    {
        // Arrange
        var (start, end) =
            ViaDateTimeRangeTestDataFactory
                .CreateInvalidDateRange_InvalidStartTime(); // This example uses start time before 8 AM as the base scenario

        // Modify 'end' to also violate the duration rule, making the test check for two failures.
        end = start.AddHours(11); // Extend to more than 10 hours to violate another rule

        // Act
        var result = ViaDateTimeRange.Create(start, end);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.OperationErrors.Count >= 2,
            "Expected at least two validation errors."); // Ensure at least two errors are returned

        // Optionally, assert specific error codes or messages if necessary
        Assert.Contains(result.OperationErrors,
            error => error is {Message: not null, Code: ErrorCode.BadRequest} &&
                     error.Message.Contains("Events cannot start before 08:00 AM."));
        Assert.Contains(result.OperationErrors,
            error => error is {Message: not null, Code: ErrorCode.BadRequest} &&
                     error.Message.Contains("The event duration must be at least 1 hour and no more than 10 hours."));
    }
}