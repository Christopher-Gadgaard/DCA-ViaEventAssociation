using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Locations;

public class ViaBooking: Entity<ViaEventId>
{
    public ViaBookingId ViaBookingId;
    
    private ViaLocationId _viaLocationId;
    
    private ViaEventId _viaEventId;
    
    private ViaStartDate _viaStartDate;
    
    private ViaEndDate _viaEndDate;

    public ViaBooking()
    {
        
    }
    private ViaBooking(ViaBookingId viaBookingId, ViaLocationId viaLocationId, ViaEventId viaEventId, ViaStartDate viaStartDate, ViaEndDate viaEndDate)
    {
        ViaBookingId = viaBookingId;
        _viaLocationId = viaLocationId;
        _viaEventId = viaEventId;
        _viaStartDate = viaStartDate;
        _viaEndDate = viaEndDate;
    }

    public static OperationResult<ViaBooking> Create(ViaBookingId viaBookingId, ViaLocationId viaLocationId,
        ViaEventId viaEventId, ViaStartDate viaStartDate, ViaEndDate viaEndDate)
    {

        if (viaBookingId == null || viaLocationId == null || viaEventId == null || viaStartDate == null ||
            viaEndDate == null)
        {
            return OperationResult<ViaBooking>.Failure(new List<OperationError>
                { new OperationError(ErrorCode.InvalidInput, "None of the parameters can be null.") });
        }

        return OperationResult<ViaBooking>.Success(new ViaBooking(viaBookingId, viaLocationId, viaEventId, viaStartDate, viaEndDate));


    }

}