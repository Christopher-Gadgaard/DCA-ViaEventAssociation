using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Contracts;

public interface IViaRepository<T, ViaId>
{
 
    public OperationResult<T> GetById(ViaId id);
    public OperationResult<T> Add(T entity);    
    public OperationResult<T> Update(T entity);
    public OperationResult<T> Delete(ViaId id);
    public OperationResult<T> GetAll();
    

}