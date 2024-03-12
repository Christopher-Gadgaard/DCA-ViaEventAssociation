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
            
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);
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
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);
            Assert.True(dateTimeRangeResult.IsSuccess); // Confirm the datetime range creation is successful
            var updateResult = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.True(updateResult.IsSuccess); // Confirm the update operation is successful
            Assert.Equal(start, viaEvent.DateTimeRange?.StartValue); // Confirm the start time matches the expected value
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
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);
            Assert.True(dateTimeRangeResult.IsSuccess); // Confirm the datetime range creation is successful
            var updateResult = viaEvent.UpdateDateTimeRange(dateTimeRangeResult.Payload!);

            // Assert
            Assert.True(updateResult.IsSuccess); // Confirm the update operation is successful
            Assert.Equal(start, viaEvent.DateTimeRange?.StartValue); // Confirm the start time matches the expected value
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
        [MemberData(nameof(DateTimeRangesWithTenHoursOrLessDuration), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Success_WhenDurationIsTenHoursOrLess(DateTime start, DateTime end)
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build(); 
            
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);
            Assert.True(dateTimeRangeResult.IsSuccess, "DateTimeRange creation should be successful for durations of 10 hours or less.");
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
            Assert.False(dateTimeRangeResult.IsSuccess, "Expected DateTimeRange creation to fail when start date is after the end date.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Message != null && error.Message.Contains("The start time must be before the end time."));
        }
    }
    
    public class F2
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithStartTimeAfterEndTimeSameDay), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenStartTimeIsAfterEndTimeSameDay(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess, "Expected DateTimeRange creation to fail when start time is after end time on the same day.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Message != null && error.Message.Contains("The start time must be before the end time."));
        }
    }
    
    public class F3
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithDurationLessThanOneHour), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenEventDurationIsLessThanOneHour(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess, "Expected DateTimeRange creation to fail when the event duration is less than one hour.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Message != null && error.Message.Contains("The event duration must be at least 1 hour"));
        }
    }

    public class F4
    {
        [Theory]
        [MemberData(nameof(DateTimeRangesWithInvalidMidnightCrossing), MemberType = typeof(ViaEventUpdateTimeRangeTests))]
        public void UpdateTimeRange_Failure_WhenDurationIsLessThanOneHourAroundMidnight(DateTime start, DateTime end)
        {
            // Act
            var dateTimeRangeResult = ViaDateTimeRange.Create(start, end);

            // Assert
            Assert.False(dateTimeRangeResult.IsSuccess, "Expected DateTimeRange creation to fail when the duration is less than one hour around midnight.");
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Code == ErrorCode.BadRequest);
            Assert.Contains(dateTimeRangeResult.OperationErrors, error => error.Message != null && error.Message.Contains("The event duration must be at least 1 hour"));
        }
    }
    
    // Data for S1 scenario
    public static IEnumerable<object[]> DateTimeRanges => new List<object[]>
    {
        new object[] { new DateTime(2023, 08, 25, 19, 00, 00), new DateTime(2023, 08, 25, 23, 59, 00) },
        new object[] { new DateTime(2023, 08, 25, 12, 00, 00), new DateTime(2023, 08, 25, 16, 30, 00) },
        new object[] { new DateTime(2023, 08, 25, 08, 00, 00), new DateTime(2023, 08, 25, 12, 15, 00) },
        new object[] { new DateTime(2023, 08, 25, 10, 00, 00), new DateTime(2023, 08, 25, 20, 00, 00) },
        new object[] { new DateTime(2023, 08, 25, 13, 00, 00), new DateTime(2023, 08, 25, 23, 00, 00) }
    };
    
    // Data for S2 scenario
    public static IEnumerable<object[]> DateTimeRangesForCrossDay => new List<object[]>
    {
        new object[] { new DateTime(2023, 08, 25, 19, 00, 00), new DateTime(2023, 08, 26, 01, 00, 00) },
        new object[] { new DateTime(2023, 08, 25, 12, 00, 00), new DateTime(2023, 08, 25, 16, 30, 00) },
        new object[] { new DateTime(2023, 08, 25, 08, 00, 00), new DateTime(2023, 08, 25, 12, 15, 00) }
    };
    
    // Data for S4 scenario: Times in the future that are valid according to S1, S2 criteria
    public static IEnumerable<object[]> DateTimeRangesForFutureStart => new List<object[]>
    {
        new object[] { new DateTime(2050, 01, 01, 10, 00, 00), new DateTime(2050, 01, 01, 15, 00, 00) },
        new object[] { new DateTime(2050, 02, 01, 12, 00, 00), new DateTime(2050, 02, 01, 18, 00, 00) },
    };
    
    // Data for S5 scenario: Durations of 10 hours or less
    public static IEnumerable<object[]> DateTimeRangesWithTenHoursOrLessDuration => new List<object[]>
    {
        // Using fixed future dates with a duration of exactly 10 hours
        new object[] { new DateTime(2050, 01, 01, 9, 00, 00), new DateTime(2050, 01, 01, 19, 00, 00) },
        // Using a duration of less than 10 hours
        new object[] { new DateTime(2050, 01, 02, 10, 00, 00), new DateTime(2050, 01, 02, 15, 30, 00) },
    };
    
    // Data for F1 scenario: Start date is after the end date
    public static IEnumerable<object[]> DateTimeRangesWithStartDateAfterEndDate => new List<object[]>
    {
        new object[] { new DateTime(2023, 08, 26, 19, 00, 00), new DateTime(2023, 08, 25, 01, 00, 00) },
        new object[] { new DateTime(2023, 08, 26, 19, 00, 00), new DateTime(2023, 08, 25, 23, 59, 00) },
        new object[] { new DateTime(2023, 08, 27, 12, 00, 00), new DateTime(2023, 08, 25, 16, 30, 00) },
        new object[] { new DateTime(2023, 08, 01, 08, 00, 00), new DateTime(2023, 07, 31, 12, 15, 00) },
    };
    
    // Data for F2 scenario: Start time is after the end time on the same day
    public static IEnumerable<object[]> DateTimeRangesWithStartTimeAfterEndTimeSameDay => new List<object[]>
    {
        new object[] { new DateTime(2023, 08, 26, 19, 00, 00), new DateTime(2023, 08, 26, 14, 00, 00) },
        new object[] { new DateTime(2023, 08, 26, 16, 00, 00), new DateTime(2023, 08, 26, 00, 00, 00) },
        new object[] { new DateTime(2023, 08, 26, 19, 00, 00), new DateTime(2023, 08, 26, 18, 59, 00) },
        new object[] { new DateTime(2023, 08, 26, 12, 00, 00), new DateTime(2023, 08, 26, 10, 10, 00) },
        // This case is technically impossible since it suggests an event starting and "ending" before it starts on the same day.
        new object[] { new DateTime(2023, 08, 26, 08, 00, 00), new DateTime(2023, 08, 26, 00, 30, 00) },
    };
    
    // Data for F3 scenario: Event duration is less than 1 hour
    public static IEnumerable<object[]> DateTimeRangesWithDurationLessThanOneHour => new List<object[]>
    {
        // The following examples set the start and end times on the same date but with less than 1-hour difference.
        new object[] { new DateTime(2023, 08, 26, 14, 00, 00), new DateTime(2023, 08, 26, 14, 50, 00) },
        new object[] { new DateTime(2023, 08, 26, 18, 00, 00), new DateTime(2023, 08, 26, 18, 59, 00) },
        new object[] { new DateTime(2023, 08, 26, 12, 00, 00), new DateTime(2023, 08, 26, 12, 30, 00) },
        new object[] { new DateTime(2023, 08, 26, 08, 00, 00), new DateTime(2023, 08, 26, 08, 59, 59) },
    };
    
    // Data for F4 scenario: Duration less than 1 hour around midnight
    public static IEnumerable<object[]> DateTimeRangesWithInvalidMidnightCrossing => new List<object[]>
    {
        // Spanning midnight but with a duration of less than 1 hour.
        new object[] { new DateTime(2023, 08, 25, 23, 30, 00), new DateTime(2023, 08, 26, 00, 15, 00) },
        new object[] { new DateTime(2023, 08, 30, 23, 01, 00), new DateTime(2023, 08, 31, 00, 00, 00) },
        new object[] { new DateTime(2023, 08, 30, 23, 59, 00), new DateTime(2023, 08, 31, 00, 01, 00) },
    };
}