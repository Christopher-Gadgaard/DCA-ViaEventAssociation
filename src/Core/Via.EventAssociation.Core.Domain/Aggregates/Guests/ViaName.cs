using System.Text.RegularExpressions;
using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;


public class ViaName : ValueObject
{
    
    public string Value { get; private init; }
    
    private ViaName(string value)
    {
        Value = value;
      
    }
    public static OperationResult<ViaName> Create(string name)
    {
        var validation = Validate(name);
        if (validation.IsSuccess)
        {
            return new ViaName(name);
        }
        return validation.OperationErrors;
    }
    
    private static OperationResult<string> Validate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return OperationResult<string>.Failure(new List<OperationError> { new OperationError(ErrorCode.InvalidInput, "name cannot be null or empty.") });
        }

        var nameRegex = "^[A-Z][a-z]{1,23}$";
        if (!Regex.IsMatch(name, nameRegex))
        {
            return OperationResult<string>.Failure(new List<OperationError> { new OperationError(ErrorCode.InvalidInput, "name format is invalid or does not meet the specific criteria.") });
        }

        return OperationResult<string>.Success(name);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}