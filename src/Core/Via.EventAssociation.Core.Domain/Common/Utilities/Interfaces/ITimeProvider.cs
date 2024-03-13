namespace Via.EventAssociation.Core.Domain.Common.Utilities.Interfaces;

public interface ITimeProvider
{
    DateTime Now { get; }
}