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

    public static OperationResult<ViaLocation> Create(ViaLocationId locationId, ViaLocationName locationName,
        ViaLocationCapacity locationCapacity)
    {
        return new ViaLocation(locationId, locationName, locationCapacity);
    }

    public OperationResult<ViaLocationName> UpdateName(ViaLocationName name)
    {
        _viaLocationName = name;
        
        return OperationResult<ViaLocationName>.Success(name);
    }

    public OperationResult<ViaLocationCapacity> UpdateLocationCapacity(ViaLocationCapacity capacity)
    {
        _viaLocationCapacity= capacity;
        return OperationResult<ViaLocationCapacity>.Success(capacity);
    }
}