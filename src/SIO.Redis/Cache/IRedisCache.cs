using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace SIO.Redis.Cache
{
    public interface IRedisCache
    {
        Task<RedisValue> GetAsync(RedisKey key, CancellationToken token = default);
        Task RemoveAsync(RedisKey key, CancellationToken token = default);
        Task RemoveAsync(RedisKey[] keys, CancellationToken token = default);
        Task SetAsync(RedisKey key, RedisValue value, CancellationToken token = default);
        Task<RedisKey[]> ScanAsync(RedisValue pattern, CancellationToken token = default);
    }
}
