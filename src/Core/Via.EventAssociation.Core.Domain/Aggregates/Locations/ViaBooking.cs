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

    
}