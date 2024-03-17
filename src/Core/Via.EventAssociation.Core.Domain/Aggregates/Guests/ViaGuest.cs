using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

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
    
    public static OperationResult<ViaGuest> Create(ViaGuestId viaGuestId, OperationResult<ViaGuestName> viaGuestNameResult, OperationResult<ViaEmail> viaEmailResult)
    {
      
        if (!viaGuestNameResult.IsSuccess)
        {
            return OperationResult<ViaGuest>.Failure(viaGuestNameResult.OperationErrors);
        }

    
        if (!viaEmailResult.IsSuccess)
        {
            return OperationResult<ViaGuest>.Failure(viaEmailResult.OperationErrors);
        }


        var guest = new ViaGuest(viaGuestId, viaGuestNameResult.Payload, viaEmailResult.Payload);
        return OperationResult<ViaGuest>.Success(guest);
    }
    
}