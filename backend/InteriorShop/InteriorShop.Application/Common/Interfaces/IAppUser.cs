namespace InteriorShop.Application.Common.Interfaces
{
    public interface IAppUser
    {
        Guid Id { get; }
        string Email { get; }
        string FullName { get; }
        string PhoneNumber { get; }
    }
}