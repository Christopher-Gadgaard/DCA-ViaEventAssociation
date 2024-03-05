using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values;

public class ViaDateTimeRange : ValueObject
{
    public DateTime StartValue { get; private init; }
    public DateTime EndValue { get; private init; }

    private ViaDateTimeRange(DateTime startValue, DateTime endValue)
    {
        StartValue = startValue;
        EndValue = endValue;
    }

    public static OperationResult<ViaDateTimeRange> Create(DateTime startValue, DateTime endValue)
    {
        var validation = Validate(startValue, endValue);
        if (validation.IsSuccess)
        {
            return new ViaDateTimeRange(startValue, endValue);
        }

        return validation.OperationErrors;
    }

    private static OperationResult Validate(DateTime startValue, DateTime endValue)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}