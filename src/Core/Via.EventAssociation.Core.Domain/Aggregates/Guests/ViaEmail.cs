using System.Text.RegularExpressions;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Contracts;
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
    
    public static  OperationResult<ViaEmail> Create(string email, ICheckEmailInUse checkEmailInUse)
    {
        var validation = Validate(email);
        if (!validation.IsSuccess)
        {
            return OperationResult<ViaEmail>.Failure(validation.OperationErrors);
        }

        bool isEmailInUse =  checkEmailInUse.IsEmailRegistered(email);
        if (isEmailInUse)
        {
            return OperationResult<ViaEmail>.Failure(new List<OperationError> { new OperationError(ErrorCode.InvalidInput, "Email is already in use.") });
        }

        return OperationResult<ViaEmail>.Success(new ViaEmail(email));
    }

    
    private static OperationResult<string> Validate(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return OperationResult<string>.Failure(new List<OperationError> { new OperationError(ErrorCode.InvalidInput, "Email cannot be null or empty.") });
        }

        var emailRegex = @"^(?:[a-zA-Z]{3,4}|\d{6})@via.dk$";
        if (!Regex.IsMatch(email, emailRegex))
        {
            return OperationResult<string>.Failure(new List<OperationError> { new OperationError(ErrorCode.InvalidInput, "Email format is invalid or does not meet the specific criteria.") });
        }

        return OperationResult<string>.Success(email);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}