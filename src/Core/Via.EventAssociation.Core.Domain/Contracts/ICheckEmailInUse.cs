namespace Via.EventAssociation.Core.Domain.Contracts;

public interface ICheckEmailInUse
{
    public bool IsEmailRegistered(string email);
}