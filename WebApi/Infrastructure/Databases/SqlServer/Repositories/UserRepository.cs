using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Domain.Utils.Useful;

namespace Infrastructure.Databases.SqlServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlServerContext _context;
        private readonly IRedisRepository _redisRepository;
        public UserRepository(SqlServerContext context, IRedisRepository redisRepository)
        {
            _context = context;
            _redisRepository = redisRepository;
        }

        #region non cached methods
        public async Task<IList<User>?> GetAllUsersAsync(int page, CancellationToken cancellationToken) => await _context.Users.AsNoTracking()
            .Include(user => user.Addresses)
            .Skip(Useful.USERS_PER_PAGE * page).Take(Useful.USERS_PER_PAGE).ToListAsync(cancellationToken);

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken) => await _context.Users.AsNoTracking()
            .Include(user => user.Addresses)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken: cancellationToken);

        public async Task<bool> CheckUserExistenceByEmailAsync(string email, CancellationToken cancellationToken) => await _context.Users.AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken: cancellationToken);

        public async Task<bool> CheckUserExistenceByIdAsync(Guid id, CancellationToken cancellationToken) => await _context.Users.AsNoTracking()
            .AnyAsync(u => u.Id == id, cancellationToken: cancellationToken);
        public async Task<int> CountAllUsers(CancellationToken cancellationToken) => await _context.Users.AsNoTracking().CountAsync(cancellationToken);
        #endregion

        #region cached methods
        public async Task<User?> GetCachedUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var cacheKey = $"{(int)Resources.Users}-{id}";
            var cachedResource = await _redisRepository.GetObjectAsync<User?>(cacheKey, () => GetUserByIdAsync(id, cancellationToken));

            return cachedResource;
        }

        public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken) => await _context.Users.AsNoTracking()
            .Include(user => user.Addresses)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);

        public async Task CreateUserAsync(User newUser, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", newUser.Id),
                new SqlParameter("@Name", newUser.Name),
                new SqlParameter("@Email", newUser.Email),
                new SqlParameter("@Logo", newUser.Logo),
                new SqlParameter("@PasswordHash", newUser.PasswordHash),
                new SqlParameter("@Salt", newUser.Salt),
                new SqlParameter("@Role", newUser.Role)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC CreateUser @Id, @Name, @Email, @Logo, @PasswordHash, @Salt, @Role",
                parameters);

            var cacheKey = $"{(int)Resources.Users}-{newUser.Id}";
            await _redisRepository.SetObjectAsync(cacheKey, newUser);
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            var parameter = new SqlParameter("@Id", id);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC DeleteUser @Id",
                parameter);

            var cacheKey = $"{(int)Resources.Users}-{id}";
            await _redisRepository.DeleteObjectAsync(cacheKey);
        }

        public async Task<User> UpdateUserAsync(Guid id, string name, byte[]? logo, CancellationToken cancellationToken)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Name", name),
                new SqlParameter("@Logo", logo)
            };

            await _context.Database.ExecuteSqlRawAsync(
               "EXEC UpdateUser @Id, @Name, @Logo",
               parameters);

            var updatedUser = await _context.Users.Include(user => user.Addresses).AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);

            var cacheKey = $"{(int)Resources.Users}-{id}";
            await _redisRepository.SetObjectAsync(cacheKey, updatedUser);

            return updatedUser;
        }
        #endregion
    }
}
