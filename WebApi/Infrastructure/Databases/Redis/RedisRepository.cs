using Domain.Interfaces.Repositories;
using Domain.Utils;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Databases.Redis
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _redis;
        public RedisRepository(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task SetObjectAsync<T>(string key, T obj, int expiresInSeconds = Useful.REDIS_DEFAULT_EXPIRES_IN_ONE_HOUR)
        {
            var json = JsonSerializer.Serialize(obj);

            Console.WriteLine($"Saving object {key} in Redis {json}.");

            await _redis.StringSetAsync(key, json);
            await _redis.KeyExpireAsync(key, TimeSpan.FromSeconds(expiresInSeconds));
        }

        public async Task<T> GetObjectAsync<T>(string key, Func<Task<T>> fetchIfNotFound)
        {
            var json = await _redis.StringGetAsync(key);

            if (!json.IsNullOrEmpty)
            {
                Console.WriteLine($"Object {key} founded in Redis.");

                return JsonSerializer.Deserialize<T>(json);
            }
            else
            {
                Console.WriteLine($"Can't found {key} in Redis.");

                var result = await fetchIfNotFound();

                if (result != null)
                {
                    await SetObjectAsync(key, result);
                }

                return result;
            }
        }

        public async Task DeleteObjectAsync(string key)
        {
            Console.WriteLine($"Deleting object {key} in Redis.");
            await _redis.KeyDeleteAsync(key);
        }
    }
}
    