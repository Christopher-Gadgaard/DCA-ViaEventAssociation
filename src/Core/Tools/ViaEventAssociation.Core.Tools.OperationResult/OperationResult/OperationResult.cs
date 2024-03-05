namespace ViaEventAssociation.Core.Tools.OperationResult.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
    public abstract class OperationResult
    {
        public List<OperationError> OperationErrors { get; protected set; } = new List<OperationError>();
        public bool IsSuccess => !OperationErrors.Any();

        public OperationResult AssertCondition(bool condition, ErrorCode errorCode)
        {
            if (!condition)
            {
                OperationErrors.Add(new OperationError(errorCode));
            }
            return this;
        }

        // Factory method for success without payload
        public static OperationResult Success() => new OperationResultWithoutPayload();

        // Factory method for failure
        public static OperationResult Failure(List<OperationError> errors) => new OperationResultWithoutPayload(errors);
    }

    // OperationResult Without Payload
    internal class OperationResultWithoutPayload : OperationResult
    {
        internal OperationResultWithoutPayload() { }
        internal OperationResultWithoutPayload(List<OperationError> errors) : base() { OperationErrors = errors; }
    }

    // Generic OperationResult Class
    public class OperationResult<T> : OperationResult
    {
        public T Payload { get; private set; } = default!;

        private OperationResult(T payload)
        {
            Payload = payload;
        }

        private OperationResult(List<OperationError> errors) : base()
        {
            OperationErrors = errors;
        }

        // Factory methods
        public static OperationResult<T> Success(T payload) => new OperationResult<T>(payload);
        public new static OperationResult<T> Failure(List<OperationError> errors) => new OperationResult<T>(errors);

        // Implicit conversions
        public static implicit operator OperationResult<T>(T payload) => Success(payload);
        public static implicit operator OperationResult<T>(OperationError error) => Failure(new List<OperationError> { error });
        public static implicit operator OperationResult<T>(List<OperationError> errors) => Failure(errors);
        
        public static OperationResult<T> Combine(params OperationResult<T>[] results)
        {
            var combinedErrors = results.SelectMany(result => result.OperationErrors).ToList();
    
            if (combinedErrors.Count != 0)
            {
                return new OperationResult<T>(combinedErrors);
            }
    
            var payload = results.Where(result => result.IsSuccess && !Equals(result.Payload, default(T)))
                .Select(result => result.Payload)
                .FirstOrDefault();
    
            return payload != null ? Success(payload) : Success(default(T)!);
        }
    }
