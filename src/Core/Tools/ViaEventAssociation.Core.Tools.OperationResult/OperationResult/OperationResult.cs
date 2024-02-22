namespace ViaEventAssociation.Core.Tools.OperationResult.OperationResult;
using OperationError;
public class OperationResult<T>
{
    public T Payload { get; private set; } = default!;
    public bool HasPayload { get; private set; }
    public List<OperationError> OperationErrors { get; } = new();
    public bool IsSuccess => !OperationErrors.Any();

    // Constructors
    private OperationResult(T payload, bool hasPayload)
    {
        Payload = payload;
        HasPayload = hasPayload;
    }

    private OperationResult(List<OperationError> operationErrors)
    {
        OperationErrors = operationErrors ?? new List<OperationError>();
    }

    // Static factory methods
    public static OperationResult<T> SuccessWithPayload(T payload) => new(payload, true);
    public static OperationResult<T> Failure(List<OperationError> operationErrors) => new(operationErrors);
    public static OperationResult<T> SuccessWithoutPayload() => new(default(T)!, false);

    // Implicit conversions 
    public static implicit operator OperationResult<T>(T payload) => SuccessWithPayload(payload);
    public static implicit operator OperationResult<T>(OperationError error) => Failure(new List<OperationError> { error });

    // Helper method to combine multiple results into a single result with or without payload
    public static OperationResult<T> Combine(params OperationResult<T>[] results)
    {
        var combinedOperationErrors = results.SelectMany(result => result.OperationErrors).ToList();
        var payloads = results.Where(result => result.HasPayload).Select(result => result.Payload).ToList();

        if (combinedOperationErrors.Any())
        {
            return new OperationResult<T>(combinedOperationErrors);
        }
        if (payloads.Any())
        {
            return new OperationResult<T>(payloads.First(), true);
        }
        return SuccessWithoutPayload();
    }
}
