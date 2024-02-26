namespace ViaEventAssociation.Core.Tools.OperationResult.OperationResult;
using OperationError;
public class OperationResult<T>
{
    public T Payload { get; private set; } = default!;
    public List<OperationError> OperationErrors { get; } = new();
    public bool IsSuccess => !OperationErrors.Any();

    // Constructors
    private OperationResult(T payload)
    {
        Payload = payload;
    }


    private OperationResult(List<OperationError> operationErrors)
    {
        OperationErrors = operationErrors ?? new List<OperationError>();
    }

    // Static factory methods
    public static OperationResult<T> SuccessWithPayload(T payload) => new(payload);
    public static OperationResult<T> SuccessWithoutPayload() => new(default(T)!);
    public static OperationResult<T> Failure(List<OperationError> operationErrors) => new(operationErrors);


    // Implicit conversions 
    public static implicit operator OperationResult<T>(T payload) => SuccessWithPayload(payload);
    public static implicit operator OperationResult<T>(OperationError error) => Failure(new List<OperationError> { error });

    // Helper method to combine multiple results into a single result with or without payload
    public static OperationResult<T> Combine(params OperationResult<T>[] results)
    {
        var combinedOperationErrors = results.SelectMany(result => result.OperationErrors).ToList();
        // Assuming T can be nullable, we filter out results where Payload is not default(T)
        var payloadResults = results.Where(result => !Equals(result.Payload, default(T))).ToList();

        if (combinedOperationErrors.Any())
        {
            return new OperationResult<T>(combinedOperationErrors);
        }
        if (payloadResults.Any())
        {
            // We take the first non-default payload as the result
            return new OperationResult<T>(payloadResults.First().Payload);
        }
        return SuccessWithoutPayload();
    
    }
}
