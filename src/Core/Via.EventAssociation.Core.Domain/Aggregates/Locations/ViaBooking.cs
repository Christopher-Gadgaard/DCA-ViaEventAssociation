using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

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
    public ViaBooking(ViaBookingId viaBookingId, ViaLocationId viaLocationId, ViaEventId viaEventId, ViaStartDate viaStartDate, ViaEndDate viaEndDate)
    {
        ViaBookingId = viaBookingId;
        _viaLocationId = viaLocationId;
        _viaEventId = viaEventId;
        _viaStartDate = viaStartDate;
        _viaEndDate = viaEndDate;
    }
    
    
}