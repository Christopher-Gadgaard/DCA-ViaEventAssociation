using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

 public abstract class ViaId
{
    public Guid Value { get; private init; }
    
    protected ViaId(Guid value)
    {
        Value = value;
    }
    
}