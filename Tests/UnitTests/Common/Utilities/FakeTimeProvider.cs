using Via.EventAssociation.Core.Domain.Common.Utilities.Interfaces;

namespace UnitTests.Common.Utilities;

public class FakeTimeProvider : ITimeProvider
{
    public DateTime Now { get; private set; }

    public FakeTimeProvider(DateTime fakeNow)
    {
        Now = fakeNow;
    }
}