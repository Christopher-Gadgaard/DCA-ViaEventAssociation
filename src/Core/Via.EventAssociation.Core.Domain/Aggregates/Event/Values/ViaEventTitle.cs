using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event.Values;

public class ViaEventTitle : ValueObject
{
    public string Value { get; private init; }

    private ViaEventTitle(string value)
    {
        Value = value;
    }

    public static OperationResult<ViaEventTitle> Create(string title)
    {
        var validation = Validate(title);
        if (validation.IsSuccess)
        {
            return new ViaEventTitle(title);
        }

        return validation.OperationErrors;
    }

    private static OperationResult<string> Validate(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length < 3 || title.Length > 75)
        {
            return new OperationError(ErrorCode.InvalidInput, "Title must be between 3 and 75 characters long.");
        }

        return OperationResult<string>.Success(title);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}