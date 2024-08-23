using Domain.Utils;

namespace Domain.Interfaces.Repositories
{
    public interface IRedisRepository
    {
        Task SetObjectAsync<T>(string key, T obj, int expiresInSeconds = Useful.REDIS_DEFAULT_EXPIRES_IN_ONE_HOUR);
        Task<T> GetObjectAsync<T>(string key, Func<Task<T>> fetchIfNotFound);
        Task DeleteObjectAsync(string key);
    }
}
