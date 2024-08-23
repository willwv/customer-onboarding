using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IAddressRepository
    {
        Task CreateAddressAsync(Address newAddress, CancellationToken cancellationToken);
        Task DeleteAddressAsync(Guid id, CancellationToken cancellationToken);
        Task<Address> UpdateAddressAsync(Guid id, string street, string number, string neighborhood, string city, string postalCode, string country, string complement, CancellationToken cancellationToken);
        Task<bool> CheckAddressExistenceByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
