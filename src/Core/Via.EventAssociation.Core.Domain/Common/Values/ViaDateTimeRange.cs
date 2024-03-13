using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Utilities;
using Via.EventAssociation.Core.Domain.Common.Utilities.Interfaces;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values;

public class ViaDateTimeRange : ValueObject
{
    public DateTime StartValue { get; private init; }
    public DateTime EndValue { get; private init; }
    
    private static ITimeProvider _timeProvider;

    private ViaDateTimeRange(DateTime startValue, DateTime endValue)
    {
        StartValue = startValue;
        EndValue = endValue;
    }
   

    internal static OperationResult<ViaDateTimeRange> Create(DateTime startValue, DateTime endValue, ITimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? new SystemTimeProvider();
        var validation = Validate(startValue, endValue);
        if (validation.IsSuccess)
        {
            return new ViaDateTimeRange(startValue, endValue);
        }

        return validation.OperationErrors;
    }
    
    private static OperationResult Validate(DateTime startValue, DateTime endValue)
    {
        var errors = new List<OperationError>();
        
        ValidateStartBeforeEndTime(startValue, endValue, errors);
        ValidateEventDuration(startValue, endValue, errors);
        ValidateStartTime(startValue, errors);
        ValidateEndTimeIfNextDay(startValue, endValue, errors);
        ValidateStartTimeIsNotInThePast(startValue, errors);

        return errors.Count != 0 ? OperationResult<ViaDateTimeRange>.Failure(errors) : OperationResult<ViaDateTimeRange>.Success(new ViaDateTimeRange(startValue, endValue));
    }

    private static void ValidateStartBeforeEndTime(DateTime startValue, DateTime endValue, ICollection<OperationError> errors)
    {
        if (startValue >= endValue)
        {
            errors.Add(new OperationError(ErrorCode.BadRequest, "The start time must be before the end time."));
        }
    }

    private static void ValidateEventDuration(DateTime startValue, DateTime endValue, ICollection<OperationError> errors)
    {
        var duration = endValue - startValue;
        if (duration < TimeSpan.FromHours(1) || duration > TimeSpan.FromHours(10))
        {
            errors.Add(new OperationError(ErrorCode.BadRequest, "The event duration must be at least 1 hour and no more than 10 hours."));
        }
    }

    private static void ValidateStartTime(DateTime startValue, ICollection<OperationError> errors)
    {
        if (startValue.TimeOfDay < new TimeSpan(8, 0, 0))
        {
            errors.Add(new OperationError(ErrorCode.BadRequest, "Events cannot start before 08:00 AM."));
        }
    }

    private static void ValidateEndTimeIfNextDay(DateTime startValue, DateTime endValue, ICollection<OperationError> errors)
    {
        if (startValue.Date < endValue.Date && endValue.TimeOfDay > new TimeSpan(1, 0, 0))
        {
            errors.Add(new OperationError(ErrorCode.BadRequest, "If the event spans to the next day, it must end by 01:00 AM."));
        }
    }

    private static void ValidateStartTimeIsNotInThePast(DateTime startValue, ICollection<OperationError> errors)
    {
        if (startValue < _timeProvider.Now)
        {
            errors.Add(new OperationError(ErrorCode.BadRequest, "The start time cannot be in the past."));
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}