using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace Via.EventAssociation.Core.Domain.Contracts;

public interface IViaGuestRepository: IViaRepository<ViaGuest, ViaId>
{
    
}