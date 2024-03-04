using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;

public class ViaGuest:AggregateRoot<ViaId>
{
    public ViaGuestId ViaGuestId;
    private ViaGuestName _viaGuestName;
    private ViaEmail _viaEmail;


    public ViaGuest()
    {
        
    }
}