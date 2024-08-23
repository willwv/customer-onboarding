using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IList<User>?> GetAllUsersAsync(int page, CancellationToken cancellationToken);
        Task<int> CountAllUsers(CancellationToken cancellationToken);
        Task<User?> GetCachedUserByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> CheckUserExistenceByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> CheckUserExistenceByIdAsync(Guid id, CancellationToken cancellationToken);
        Task CreateUserAsync(User newUser, CancellationToken cancellationToken);
        Task<User> UpdateUserAsync(Guid id, string name, byte[]? logo, CancellationToken cancellationToken);
        Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);
    }
}
