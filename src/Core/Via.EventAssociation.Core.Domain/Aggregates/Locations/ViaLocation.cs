using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaLocation : AggregateRoot<ViaId>
{
    public ViaLocationId ViaLocationId { get; private init; }
    public ViaLocationName ViaLocationName { get; private set; }
    public ViaLocationCapacity ViaLocationCapacity { get; private set; }

    public ViaLocation()
    {
    }

    public ViaLocation(ViaLocationId locationId, ViaLocationName locationName, ViaLocationCapacity locationCapacity)
    {
        ViaLocationId = locationId;
        ViaLocationName = locationName;
        ViaLocationCapacity = locationCapacity;
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
            this.ViaLocationName = result.Payload;
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
            this.ViaLocationCapacity = result.Payload;
            return OperationResult<ViaLocationCapacity>.Success(result.Payload);
        }
        else
        {
            return OperationResult<ViaLocationCapacity>.Failure(result.OperationErrors);
        }
    }
}