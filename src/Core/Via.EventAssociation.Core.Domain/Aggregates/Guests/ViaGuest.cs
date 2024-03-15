using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;

public class ViaGuest:AggregateRoot<ViaGuestId>
{
    
    private ViaGuestName _viaGuestName;
    private ViaEmail _viaEmail;

    internal new ViaGuestId Id => base.Id;
    internal ViaGuestName ViaGuestName => _viaGuestName;
    internal ViaEmail ViaEmail => _viaEmail;
    
 
    public ViaGuest( ViaGuestId viaGuestId, ViaGuestName viaGuestName, ViaEmail viaEmail):base(viaGuestId)
    {
        _viaGuestName = viaGuestName;
        _viaEmail = viaEmail;
    }
    
    public static ViaGuest Create(ViaGuestId viaGuestId, ViaGuestName viaGuestName, ViaEmail viaEmail)
    {
        return new ViaGuest(viaGuestId, viaGuestName, viaEmail);
    }
    
}