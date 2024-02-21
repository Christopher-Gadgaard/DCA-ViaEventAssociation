namespace ViaEventAssociation.Core.Tools.OperationResult.OperationResult;
using OperationError;
public class OperationResult<T>
{
    public T Payload { get; } = default!;
    public List<OperationError> OperationErrors { get; } = new();
    public bool IsSuccess => OperationErrors.Count == 0;

    // Constructor for success
    public OperationResult(T payload) => Payload = payload;

    // Constructor for OperationErrors
    public OperationResult(List<OperationError> operationErrors) => OperationErrors = operationErrors ?? new List<OperationError>();

    // Static factory methods
    public static OperationResult<T> Success(T payload) => new(payload);

    public static OperationResult<T> Failure(List<OperationError> operationErrors) => new(operationErrors);
    
    // Implicit conversion from T to OperationResult<T> for success
    public static implicit operator OperationResult<T>(T payload) => Success(payload);

    // Implicit conversion from OperationError to OperationResult<T> for failure
    public static implicit operator OperationResult<T>(OperationError error) => Failure(new List<OperationError> { error });

    
    // Helper method to combine multiple results 
    public static OperationResult<List<T>> Combine(params OperationResult<T>[] results)
    {
        var combinedOperationErrors = results.SelectMany(result => result.OperationErrors).ToList();
        if (combinedOperationErrors.Any())
            return new OperationResult<List<T>>(combinedOperationErrors);
        
        var payloads = results.Select(result => result.Payload).ToList();
        return new OperationResult<List<T>>(payloads);
    }
}
