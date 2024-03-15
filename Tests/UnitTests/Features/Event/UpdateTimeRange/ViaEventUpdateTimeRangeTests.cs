using UnitTests.Common.Utilities;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.UpdateTimeRange;

public class ViaEventUpdateTimeRangeTests
{
    public class S1
    {
        [Theory]
        [MemberData(nameof(DateTimeRanges), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Success_WhenEventIsDraft(DateTime start, DateTime end)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build();

            var fakeTimeProvider = new FakeTimeProvider(new DateTime(2022, 08, 25, 10, 00, 00));
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end, fakeTimeProvider);
            Assert.True(dateTimeRangeResult.IsSuccess);

            // Act
            var result = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(dateTimeRangeResult.Payload.StartValue, viaEvent.DateTimeRange?.StartValue);
            Assert.Equal(dateTimeRangeResult.Payload.EndValue, viaEvent.DateTimeRange?.EndValue);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Remains Draft if it was Draft
        }
    }

    public class S2
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesForCrossDay), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Success_WhenEventSpansToNextDayButEndsBefore1AM(DateTime start, DateTime end)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithStatus(ViaEventStatus.Draft) // Ensure the event is in a draft state
                .Build();

            // Act
            var fakeTimeProvider = new FakeTimeProvider(new DateTime(2022, 08, 25, 10, 00, 00));
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end, fakeTimeProvider);
            Assert.True(dateTimeRangeResult.IsSuccess); // Confirm the datetime range creation is successful
            var updateResult = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.True(updateResult.IsSuccess); // Confirm the update operation is successful
            Assert.Equal(start,
                viaEvent.DateTimeRange?.StartValue); // Confirm the start time matches the expected value
            Assert.Equal(end, viaEvent.DateTimeRange?.EndValue); // Confirm the end time matches the expected value
        }
    }

    public class S3
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesForCrossDay), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Success_AndRevertsToDraft_WhenEventIsReady(DateTime start, DateTime end)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithStatus(ViaEventStatus.Ready) // Ensure the event is in a ready state
                .Build();

            // Act
            var fakeTimeProvider = new FakeTimeProvider(new DateTime(2022, 08, 25, 10, 00, 00));
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end, fakeTimeProvider);
            Assert.True(dateTimeRangeResult.IsSuccess); // Confirm the datetime range creation is successful
            var updateResult = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.True(updateResult.IsSuccess); // Confirm the update operation is successful
            Assert.Equal(start,
                viaEvent.DateTimeRange?.StartValue); // Confirm the start time matches the expected value
            Assert.Equal(end, viaEvent.DateTimeRange?.EndValue); // Confirm the end time matches the expected value
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status); // Ensure the event status is reverted to Draft
        }
    }

    public class S4
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesForFutureStart), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Success_WhenStartTimeIsInFuture(DateTime start, DateTime end)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build(); // Assuming default status is Draft which is modifiable

            // Ensure the start time is indeed in the future relative to the current time
            Assert.True(start > DateTime.UtcNow, "Start time must be in the future for this test case.");

            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);
            Assert.True(dateTimeRangeResult.IsSuccess, "DateTimeRange creation should be successful.");
            var updateResult = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.True(updateResult.IsSuccess, "Expected the DateTimeRange update to be successful.");
            Assert.Equal(start, viaEvent.DateTimeRange?.StartValue);
            Assert.Equal(end, viaEvent.DateTimeRange?.EndValue);
        }
    }

    public class S5
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithTenHoursOrLessDuration),
            MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Success_WhenDurationIsTenHoursOrLess(DateTime start, DateTime end)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build();

            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);
            Assert.True(dateTimeRangeResult.IsSuccess,
                "DateTimeRange creation should be successful for durations of 10 hours or less.");
            var updateResult = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.True(updateResult.IsSuccess, "Expected the DateTimeRange update to be successful.");
            Assert.Equal(start, viaEvent.DateTimeRange?.StartValue);
            Assert.Equal(end, viaEvent.DateTimeRange?.EndValue);
        }
    }

    public class F1
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithStartDateAfterEndDate), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenStartDateIsAfterEndDate(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess,
                "Expected DateTimeRange creation to fail when start date is after the end date.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The start time must be before the end time."));
        }
    }

    public class F2
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithStartTimeAfterEndTimeSameDay),
            MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenStartTimeIsAfterEndTimeSameDay(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess,
                "Expected DateTimeRange creation to fail when start time is after end time on the same day.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The start time must be before the end time."));
        }
    }

    public class F3
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithDurationLessThanOneHour),
            MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenEventDurationIsLessThanOneHour(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess,
                "Expected DateTimeRange creation to fail when the event duration is less than one hour.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null && error.Message.Contains("The event duration must be at least 1 hour"));
        }
    }

    public class F4
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithInvalidMidnightCrossing),
            MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenDurationIsLessThanOneHourAroundMidnight(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess,
                "Expected DateTimeRange creation to fail when the duration is less than one hour around midnight.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null && error.Message.Contains("The event duration must be at least 1 hour"));
        }
    }

    public class F5
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithStartTimeBefore8Am), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenStartTimeIsBefore8AM(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess,
                "Expected DateTimeRange creation to fail when start time is before 8 AM.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null && error.Message.Contains("Events cannot start before 08:00 AM"));
        }
    }

    public class F6
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithInvalidEarlyMorningSpan),
            MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenEventSpansIntoRestrictedEarlyMorningHours(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.True(dateTimeRangeResult.IsFailure,
                "Expected DateTimeRange creation to fail when the event spans into restricted early morning hours.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("If the event spans to the next day, it must end by 01:00 AM."));
        }
    }

    public class F7
    {
        [Fact] // Using Fact here since we're not iterating over data but testing a specific behavior
        public void UpdateTimeRange_Failure_WhenEventIsActive()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Test Title")
                .WithStatus(ViaEventStatus.Active) // Set the event to Active status
                .Build();

            // Here, we're using fixed times, but the specific values aren't as important as the event's status
            var newStartTime = new DateTime(2050, 01, 01, 10, 00, 00);
            var newEndTime = new DateTime(2050, 01, 01, 14, 00, 00);

            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(newStartTime, newEndTime);
            Assert.True(dateTimeRangeResult.IsSuccess, "DateTimeRange creation should succeed for valid times.");
            var updateResult = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.False(updateResult.IsSuccess, "Expected DateTimeRange update to fail for an active event.");
            Assert.Contains(updateResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(updateResult.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The event cannot be modified in its current state."));
        }
    }

    public class F8
    {
        [Fact] // Using Fact here since we're testing a specific condition, not iterating over data
        public void UpdateTimeRange_Failure_WhenEventIsCancelled()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .WithStatus(ViaEventStatus.Cancelled) // Set the event to Cancelled status
                .Build();

            // Using fixed times, the specific values are less important than the event's status
            var newStartTime = new DateTime(2050, 01, 01, 10, 00, 00);
            var newEndTime = new DateTime(2050, 01, 01, 14, 00, 00);

            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(newStartTime, newEndTime);
            Assert.True(dateTimeRangeResult.IsSuccess, "DateTimeRange creation should succeed for valid times.");
            var updateResult = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.False(updateResult.IsSuccess, "Expected DateTimeRange update to fail for a cancelled event.");
            Assert.Contains(updateResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(updateResult.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The event cannot be modified in its current state."));
        }
    }

    public class F9
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesExceedingMaximumDuration), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenDurationExceedsMaximumAllowed(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess,
                "Expected DateTimeRange creation to fail when the event duration exceeds the maximum allowed 10 hours.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains(
                             "The event duration must be at least 1 hour and no more than 10 hours."));
        }
    }

    public class F10
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithStartTimeInPast), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenStartTimeIsInPast(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess,
                "Expected DateTimeRange creation to fail when start time is set in the past.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null && error.Message.Contains("The start time cannot be in the past."));
        }
    }

    public class F11
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithInvalidOvernightSpan), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenEventSpansRestrictedOvernightHours(DateTime start, DateTime end)
        {
            // Act
            var fakeTimeProvider = new FakeTimeProvider(new DateTime(2022, 08, 25, 10, 00, 00));
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end, fakeTimeProvider);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess,
                "Expected DateTimeRange creation to fail when the event spans restricted overnight hours.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors,
                error => error.Message != null && (error.Message.Contains("Events cannot start before 08:00 AM.") ||
                                                  error.Message.Contains("If the event spans to the next day, it must end by 01:00 AM.")));
        }
    }

    // Data for S1 scenario
    public static IEnumerable<object[]> DateTimeRanges => new List<object[]>
    {
        new object[] {new DateTime(2023, 08, 25, 19, 00, 00), new DateTime(2023, 08, 25, 23, 59, 00)},
        new object[] {new DateTime(2023, 08, 25, 12, 00, 00), new DateTime(2023, 08, 25, 16, 30, 00)},
        new object[] {new DateTime(2023, 08, 25, 08, 00, 00), new DateTime(2023, 08, 25, 12, 15, 00)},
        new object[] {new DateTime(2023, 08, 25, 10, 00, 00), new DateTime(2023, 08, 25, 20, 00, 00)},
        new object[] {new DateTime(2023, 08, 25, 13, 00, 00), new DateTime(2023, 08, 25, 23, 00, 00)}
    };

    // Data for S2 scenario
    public static IEnumerable<object[]> DateTimeRangesForCrossDay => new List<object[]>
    {
        new object[] {new DateTime(2023, 08, 25, 19, 00, 00), new DateTime(2023, 08, 26, 01, 00, 00)},
        new object[] {new DateTime(2023, 08, 25, 12, 00, 00), new DateTime(2023, 08, 25, 16, 30, 00)},
        new object[] {new DateTime(2023, 08, 25, 08, 00, 00), new DateTime(2023, 08, 25, 12, 15, 00)}
    };

    // Data for S4 scenario: Times in the future that are valid according to S1, S2 criteria
    public static IEnumerable<object[]> DateTimeRangesForFutureStart => new List<object[]>
    {
        new object[] {new DateTime(2050, 01, 01, 10, 00, 00), new DateTime(2050, 01, 01, 15, 00, 00)},
        new object[] {new DateTime(2050, 02, 01, 12, 00, 00), new DateTime(2050, 02, 01, 18, 00, 00)},
    };

    // Data for S5 scenario: Durations of 10 hours or less
    public static IEnumerable<object[]> DateTimeRangesWithTenHoursOrLessDuration => new List<object[]>
    {
        // Using fixed future dates with a duration of exactly 10 hours
        new object[] {new DateTime(2050, 01, 01, 9, 00, 00), new DateTime(2050, 01, 01, 19, 00, 00)},
        // Using a duration of less than 10 hours
        new object[] {new DateTime(2050, 01, 02, 10, 00, 00), new DateTime(2050, 01, 02, 15, 30, 00)},
    };

    // Data for F1 scenario: Start date is after the end date
    public static IEnumerable<object[]> DateTimeRangesWithStartDateAfterEndDate => new List<object[]>
    {
        new object[] {new DateTime(2023, 08, 26, 19, 00, 00), new DateTime(2023, 08, 25, 01, 00, 00)},
        new object[] {new DateTime(2023, 08, 26, 19, 00, 00), new DateTime(2023, 08, 25, 23, 59, 00)},
        new object[] {new DateTime(2023, 08, 27, 12, 00, 00), new DateTime(2023, 08, 25, 16, 30, 00)},
        new object[] {new DateTime(2023, 08, 01, 08, 00, 00), new DateTime(2023, 07, 31, 12, 15, 00)},
    };

    // Data for F2 scenario: Start time is after the end time on the same day
    public static IEnumerable<object[]> DateTimeRangesWithStartTimeAfterEndTimeSameDay => new List<object[]>
    {
        new object[] {new DateTime(2023, 08, 26, 19, 00, 00), new DateTime(2023, 08, 26, 14, 00, 00)},
        new object[] {new DateTime(2023, 08, 26, 16, 00, 00), new DateTime(2023, 08, 26, 00, 00, 00)},
        new object[] {new DateTime(2023, 08, 26, 19, 00, 00), new DateTime(2023, 08, 26, 18, 59, 00)},
        new object[] {new DateTime(2023, 08, 26, 12, 00, 00), new DateTime(2023, 08, 26, 10, 10, 00)},
        // This case is technically impossible since it suggests an event starting and "ending" before it starts on the same day.
        new object[] {new DateTime(2023, 08, 26, 08, 00, 00), new DateTime(2023, 08, 26, 00, 30, 00)},
    };

    // Data for F3 scenario: Event duration is less than 1 hour
    public static IEnumerable<object[]> DateTimeRangesWithDurationLessThanOneHour => new List<object[]>
    {
        // The following examples set the start and end times on the same date but with less than 1-hour difference.
        new object[] {new DateTime(2023, 08, 26, 14, 00, 00), new DateTime(2023, 08, 26, 14, 50, 00)},
        new object[] {new DateTime(2023, 08, 26, 18, 00, 00), new DateTime(2023, 08, 26, 18, 59, 00)},
        new object[] {new DateTime(2023, 08, 26, 12, 00, 00), new DateTime(2023, 08, 26, 12, 30, 00)},
        new object[] {new DateTime(2023, 08, 26, 08, 00, 00), new DateTime(2023, 08, 26, 08, 59, 59)},
    };

    // Data for F4 scenario: Duration less than 1 hour around midnight
    public static IEnumerable<object[]> DateTimeRangesWithInvalidMidnightCrossing => new List<object[]>
    {
        // Spanning midnight but with a duration of less than 1 hour.
        new object[] {new DateTime(2023, 08, 25, 23, 30, 00), new DateTime(2023, 08, 26, 00, 15, 00)},
        new object[] {new DateTime(2023, 08, 30, 23, 01, 00), new DateTime(2023, 08, 31, 00, 00, 00)},
        new object[] {new DateTime(2023, 08, 30, 23, 59, 00), new DateTime(2023, 08, 31, 00, 01, 00)},
    };

    // Data for F5 scenario: Start time is before 8 AM
    public static IEnumerable<object[]> DateTimeRangesWithStartTimeBefore8Am => new List<object[]>
    {
        // Events starting before 08:00 AM
        new object[] {new DateTime(2023, 08, 25, 07, 50, 00), new DateTime(2023, 08, 25, 14, 00, 00)},
        new object[] {new DateTime(2023, 08, 25, 07, 59, 00), new DateTime(2023, 08, 25, 15, 00, 00)},
        new object[] {new DateTime(2023, 08, 25, 01, 01, 00), new DateTime(2023, 08, 25, 08, 30, 00)},
        new object[] {new DateTime(2023, 08, 25, 05, 59, 00), new DateTime(2023, 08, 25, 07, 59, 00)},
        // This last case is to check if an event tries to start just before 08:00 AM
        new object[] {new DateTime(2023, 08, 25, 00, 59, 00), new DateTime(2023, 08, 25, 07, 59, 00)},
    };

    // Data for F6 scenario: Event spans into the restricted early morning hours
    public static IEnumerable<object[]> DateTimeRangesWithInvalidEarlyMorningSpan => new List<object[]>
    {
        // Events starting before 01:00 AM and ending after 01:00 AM, violating the early morning restriction
        new object[] {new DateTime(2023, 08, 24, 23, 50, 00), new DateTime(2023, 08, 25, 01, 01, 00)},
        new object[] {new DateTime(2023, 08, 24, 22, 00, 00), new DateTime(2023, 08, 25, 07, 59, 00)},
        new object[] {new DateTime(2023, 08, 30, 23, 00, 00), new DateTime(2023, 08, 31, 02, 30, 00)},
        // These examples specifically test the boundary conditions around the restricted hours.
        new object[] {new DateTime(2023, 08, 24, 23, 50, 00), new DateTime(2023, 08, 25, 01, 01, 00)},
    };

    // Data for F9 scenario: Event duration exceeds 10 hours
    public static IEnumerable<object[]> DateTimeRangesExceedingMaximumDuration => new List<object[]>
    {
        // Events with durations exceeding 10 hours
        new object[] {new DateTime(2023, 08, 30, 08, 00, 00), new DateTime(2023, 08, 30, 18, 01, 00)},
        new object[] {new DateTime(2023, 08, 30, 14, 59, 00), new DateTime(2023, 08, 31, 01, 00, 00)},
        new object[] {new DateTime(2023, 08, 30, 14, 00, 00), new DateTime(2023, 08, 31, 00, 01, 00)},
        // This case tests the boundary by setting the duration exactly at the limit plus one minute
        new object[] {new DateTime(2023, 08, 30, 14, 00, 00), new DateTime(2023, 08, 31, 00, 01, 00)},
    };

    // Data for F10 scenario: Start time is set in the past
    public static IEnumerable<object[]> DateTimeRangesWithStartTimeInPast => new List<object[]>
    {
        // Using dates in the past to trigger the validation failure
        new object[] {new DateTime(2020, 01, 01, 10, 00, 00), new DateTime(2020, 01, 01, 14, 00, 00)},
        new object[] {new DateTime(2021, 02, 02, 09, 00, 00), new DateTime(2021, 02, 02, 12, 00, 00)},
    };

    // Data for F11 scenario: Event spans restricted overnight hours
    public static IEnumerable<object[]> DateTimeRangesWithInvalidOvernightSpan => new List<object[]>
    {
        // Events that start before 01:00 AM and end after 08:00 AM, violating the overnight restriction
        new object[] {new DateTime(2023, 08, 31, 00, 30, 00), new DateTime(2023, 08, 31, 08, 30, 00)},
        new object[] {new DateTime(2023, 08, 30, 23, 59, 00), new DateTime(2023, 08, 31, 08, 01, 00)},
        new object[] {new DateTime(2023, 08, 31, 01, 00, 00), new DateTime(2023, 08, 31, 08, 00, 00)},
    };
}