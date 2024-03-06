using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocation : AggregateRoot<ViaLocationId>
{
    private ViaLocationId _viaLocationId;
    private ViaLocationName _viaLocationName;
    private ViaLocationCapacity _viaLocationCapacity;


    internal new ViaLocationId Id => base.Id;
    internal new ViaLocationName Name => _viaLocationName;
    internal new ViaLocationCapacity Capacity => _viaLocationCapacity;
    
    
    

    public ViaLocation(ViaLocationId locationId, ViaLocationName locationName, ViaLocationCapacity locationCapacity)
    {

        _viaLocationName = locationName ?? ViaLocationName.Create("Location Name").Payload;
        _viaLocationCapacity = locationCapacity ?? ViaLocationCapacity.Create(20).Payload;
    }

    private static OperationResult<ViaLocation> Create(ViaLocationId locationId, ViaLocationName locationName,
        ViaLocationCapacity locationCapacity)
    {
        return new ViaLocation(locationId, locationName, locationCapacity);
    }

    public OperationResult<ViaLocationName> UpdateName(string name)
    {
        ViaLocationName updatedName = new ViaLocationName(name);
        var result = ViaLocationName.Create(name);
        if (result.IsSuccess)
        {
            this._viaLocationName = result.Payload;
            return OperationResult<ViaLocationName>.Success(result.Payload);
        }
        else
        {
            return OperationResult<ViaLocationName>.Failure(result.OperationErrors);
        }
    }

    public OperationResult<ViaLocationCapacity> UpdateLocationCapacity(int capacity)
    {
        var result = ViaLocationCapacity.Create(capacity);
        if (result.IsSuccess)
        {
            this._viaLocationCapacity = result.Payload;
            return OperationResult<ViaLocationCapacity>.Success(result.Payload);
        }
        else
        {
            return OperationResult<ViaLocationCapacity>.Failure(result.OperationErrors);
        }
    }
}