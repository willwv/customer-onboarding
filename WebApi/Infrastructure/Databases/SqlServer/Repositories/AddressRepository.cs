using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Domain.Utils.Useful;

namespace Infrastructure.Databases.SqlServer.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly SqlServerContext _context;
        private readonly IRedisRepository _redisRepository;

        public AddressRepository(SqlServerContext context, IRedisRepository redisRepository)
        {
            _context = context;
            _redisRepository = redisRepository;
        }

        public async Task<bool> CheckAddressExistenceByIdAsync(Guid id, CancellationToken cancellationToken) => await _context.Address.AsNoTracking()
            .AnyAsync(u => u.Id == id, cancellationToken: cancellationToken);

        public async Task CreateAddressAsync(Address newAddress, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", newAddress.Id),
                new SqlParameter("@UserId", newAddress.UserId),
                new SqlParameter("@Street", newAddress.Street),
                new SqlParameter("@Number", newAddress.Number),
                new SqlParameter("@Neighborhood", newAddress.Neighborhood),
                new SqlParameter("@City", newAddress.City),
                new SqlParameter("@PostalCode", newAddress.PostalCode),
                new SqlParameter("@State", newAddress.State),
                new SqlParameter("@Complement", newAddress.Complement)
            };

            await _context.Database.ExecuteSqlRawAsync(
            "EXEC CreateAddress @Id, @UserId, @Street, @Number, @Neighborhood, @City, @PostalCode, @State, @Complement",
            parameters);

            await RemoveUserFromCache(newAddress.UserId);
        }

        public async Task DeleteAddressAsync(Guid id, CancellationToken cancellationToken)
        {
            var oldAddress = await _context.Address.AsNoTracking().FirstOrDefaultAsync(address => address.Id == id, cancellationToken: cancellationToken);

            if (oldAddress != null)
            {
                await RemoveUserFromCache(oldAddress.UserId);
                await _context.Address.Where(address => address.Id == id).ExecuteDeleteAsync(cancellationToken);
            }
        }

        public async Task<Address> UpdateAddressAsync(Guid id, string street, string number, string neighborhood, string city, string postalCode, string state, string complement, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Street", street),
                new SqlParameter("@Number", number),
                new SqlParameter("@Neighborhood", neighborhood),
                new SqlParameter("@City", city),
                new SqlParameter("@PostalCode", postalCode),
                new SqlParameter("@State", state),
                new SqlParameter("@Complement", complement)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateAddress @Id, @Street, @Number, @Neighborhood, @City, @PostalCode, @State, @Complement",
                parameters);

            var updatedAddress = await _context.Address.AsNoTracking().FirstOrDefaultAsync(addr => addr.Id == id, cancellationToken: cancellationToken);

            await RemoveUserFromCache(updatedAddress!.UserId);

            return updatedAddress;
        }

        private async Task RemoveUserFromCache(Guid userId) 
        {
            var cacheKey = $"{(int)Resources.Users}-{userId}";
            await _redisRepository.DeleteObjectAsync(cacheKey);
        }
    }
}
