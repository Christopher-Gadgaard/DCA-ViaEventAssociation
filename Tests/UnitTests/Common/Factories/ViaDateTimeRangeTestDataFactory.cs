namespace UnitTests.Common.Factories;

public abstract class ViaDateTimeRangeTestDataFactory
{
    // Generate a valid date range
    public static (DateTime start, DateTime end) CreateValidDateRange()
    {
        return (new DateTime(2023, 08, 25, 10, 0, 0), new DateTime(2023, 08, 25, 20, 0, 0));
    }

    // Generate date range where start is after the end
    public static (DateTime start, DateTime end) CreateInvalidDateRange_StartAfterEnd()
    {
        return (new DateTime(2023, 08, 25, 21, 0, 0), new DateTime(2023, 08, 25, 20, 0, 0));
    }

    // Generate date range with invalid start time (before 08:00 AM)
    public static (DateTime start, DateTime end) CreateInvalidDateRange_InvalidStartTime()
    {
        return (new DateTime(2023, 08, 25, 4, 59, 0), new DateTime(2023, 08, 25, 10, 0, 0));
    }

    // Generate date range with invalid duration (more than 10 hours)
    public static (DateTime start, DateTime end) CreateInvalidDateRange_InvalidDuration()
    {
        return (new DateTime(2023, 08, 25, 10, 0, 0), new DateTime(2023, 08, 26, 1, 1, 0));
    }
    
    // Generate valid date range with end time at 01:00 AM next day
    public static (DateTime start, DateTime end) CreateValidDateRange_EndTimeAtNextDayBoundary()
    {
        return (new DateTime(2023, 08, 25, 20, 0, 0), new DateTime(2023, 08, 26, 01, 0, 0));
    }

    // Generate invalid date range with end time after 01:00 AM next day
    public static (DateTime start, DateTime end) CreateInvalidDateRange_EndTimeAfterNextDayBoundary()
    {
        return (new DateTime(2023, 08, 25, 20, 0, 0), new DateTime(2023, 08, 26, 01, 01, 0));
    }

    // Generate valid date range with start time at boundary (08:00 AM)
    public static (DateTime start, DateTime end) CreateValidDateRange_StartTimeAtBoundary()
    {
        return (new DateTime(2023, 08, 25, 08, 0, 0), new DateTime(2023, 08, 25, 18, 0, 0));
    }
    
    // Generate invalid date range with start time before boundary (07:59 AM)
    public static (DateTime start, DateTime end) CreateInvalidDateRange_StartTimeBeforeBoundary()
    {
        return (new DateTime(2023, 08, 25, 07, 59, 0), new DateTime(2023, 08, 25, 18, 0, 0));
    }
    
    // Generate invalid date range with duration less than 1 hour (59 minutes)
    public static (DateTime start, DateTime end) CreateInvalidDateRange_DurationLessThan1Hour()
    {
        return (new DateTime(2023, 08, 25, 08, 0, 0), new DateTime(2023, 08, 25, 08, 59, 0));
    }
}