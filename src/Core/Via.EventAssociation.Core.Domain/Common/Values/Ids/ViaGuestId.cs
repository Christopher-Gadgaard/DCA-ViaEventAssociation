using Via.EventAssociation.Core.Domain.Common.Bases;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaGuestId :ViaId
{
    
    private ViaGuestId(Guid value):base(value)
    {
  
    }
    
    public static ViaGuestId Create()
    {
        var id = Guid.NewGuid();
        return new ViaGuestId(id);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}