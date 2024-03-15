using Via.EventAssociation.Core.Domain.Common.Utilities.Interfaces;

namespace Via.EventAssociation.Core.Domain.Common.Utilities;

public class SystemTimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.UtcNow.ToLocalTime();
}