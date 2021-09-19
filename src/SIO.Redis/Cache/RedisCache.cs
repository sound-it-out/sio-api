using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace SIO.Redis.Cache
{
    internal sealed class RedisCache : IRedisCache
    {
        private volatile ConnectionMultiplexer _connection;
        private IDatabase _cache;

        private readonly RedisOptions _options;
        private readonly ILogger<RedisCache> _logger;
        private readonly SemaphoreSlim _connectionLock;

        public RedisCache(IOptions<RedisOptions> options,
            ILogger<RedisCache> logger)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _options = options.Value;
            _logger = logger;

            _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        }


        public async Task<RedisValue> GetAsync(RedisKey key, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisCache)}.{nameof(GetAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await ConnectAsync(cancellationToken);
            return await _cache.StringGetAsync(key);
        }

        public async Task RemoveAsync(RedisKey key, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisCache)}.{nameof(RemoveAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await ConnectAsync(cancellationToken);
            await _cache.KeyDeleteAsync(key);
        }

        public async Task RemoveAsync(RedisKey[] keys, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisCache)}.{nameof(RemoveAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await ConnectAsync(cancellationToken);
            await _cache.KeyDeleteAsync(keys);
        }

        public async Task<RedisKey[]> ScanAsync(RedisValue pattern, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisCache)}.{nameof(ScanAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await ConnectAsync(cancellationToken);

            var servers = _connection.GetEndPoints().Select(ep => _connection.GetServer(ep)).ToArray();

            if (!servers.Any())
                return Array.Empty<RedisKey>();

            var keys = new List<RedisKey>();

            foreach (var server in servers)
                await foreach (var key in server.KeysAsync(_cache.Database, pattern, 100))
                    keys.Add(key);

            return keys.ToArray();
        }

        public async Task SetAsync(RedisKey key, RedisValue value, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisCache)}.{nameof(SetAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await ConnectAsync(cancellationToken);
            await _cache.StringSetAsync(key, value);
        }

        private async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisCache)}.{nameof(ConnectAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (_cache != null)
            {
                return;
            }

            await _connectionLock.WaitAsync(cancellationToken);

            try
            {
                if (_cache == null)
                {
                    _connection = await ConnectionMultiplexer.ConnectAsync(_options.ConnectionString);
                    _cache = _connection.GetDatabase();
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }

    }
}
