using System.Text.RegularExpressions;
using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;

public class ViaEmail:ValueObject
{
    public string Value { get; private init; }
    
    private ViaEmail(string value)
    {
        Value = value;
        Validate(value);
    }
    
    public static OperationResult<ViaEmail> Create(string email)
    {
        var validation = Validate(email);
        if (validation.IsSuccess)
        {
            return new ViaEmail(email);
        }

        return validation.OperationErrors;
    }
    
    private static OperationResult<string> Validate(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return OperationResult<string>.Failure(new List<OperationError> { new OperationError(ErrorCode.InvalidInput, "Email cannot be null or empty.") });
        }

        // Adjusted regex pattern based on the specified requirements
        var emailRegex = @"^(?:[a-zA-Z]{3,4}|\d{6})@[a-zA-Z0-9.-]+\.via\.dk$";
        if (!Regex.IsMatch(email, emailRegex))
        {
            return OperationResult<string>.Failure(new List<OperationError> { new OperationError(ErrorCode.InvalidInput, "Email format is invalid or does not meet the specific criteria.") });
        }

        return OperationResult<string>.SuccessWithPayload(email);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}