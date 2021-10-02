using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIO.Infrastructure.Projections;
using SIO.Infrastructure.Serialization;
using SIO.Redis.Cache;

namespace SIO.Redis.Projections
{
    public class RedisProjectionWriter<TView> : IProjectionWriter<TView>
        where TView : class, IProjection
    {
        private readonly IRedisCache _cache;
        private readonly ILogger<RedisProjectionWriter<TView>> _logger;
        private readonly IEventSerializer _eventSerializer;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly string _keyPrefix;

        public RedisProjectionWriter(IRedisCache cache,
            ILogger<RedisProjectionWriter<TView>> logger,
            IEventSerializer eventSerializer,
            IEventDeserializer eventDeserializer)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (eventSerializer == null)
                throw new ArgumentNullException(nameof(eventSerializer));
            if (eventDeserializer == null)
                throw new ArgumentNullException(nameof(eventDeserializer));

            _cache = cache;
            _logger = logger;
            _eventSerializer = eventSerializer;
            _keyPrefix = typeof(TView).Name;
        }

        public async Task<TView> AddAsync(string subject, Func<TView> add, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisProjectionWriter<TView>)}.{nameof(AddAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var view = add();
            await _cache.SetAsync(BuildKey(subject), _eventSerializer.Serialize(view), cancellationToken);
            return view;
        }

        public async Task RemoveAsync(string subject, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisProjectionWriter<TView>)}.{nameof(RemoveAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            await _cache.RemoveAsync(BuildKey(subject), cancellationToken);
        }

        public async Task<TView> UpdateAsync(string subject, Func<TView, TView> update, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisProjectionWriter<TView>)}.{nameof(UpdateAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var view = _eventDeserializer.Deserialize<TView>(await _cache.GetAsync(BuildKey(subject), cancellationToken));
            update(view);
            await _cache.SetAsync(BuildKey(subject), _eventSerializer.Serialize(view), cancellationToken);
            return view;
        }

        public async Task<TView> UpdateAsync(string subject, Action<TView> update, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisProjectionWriter<TView>)}.{nameof(UpdateAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var view = await UpdateAsync(subject, view =>
            {
                update(view);
                return view;
            }, cancellationToken);

            return view;
        }

        public async Task ResetAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisProjectionWriter<TView>)}.{nameof(ResetAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var keys = await _cache.ScanAsync(BuildKey("*"), cancellationToken);
            await _cache.RemoveAsync(keys, cancellationToken);
        }

        public async Task<TView> RetrieveAsync(string subject, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"{nameof(RedisProjectionWriter<TView>)}.{nameof(UpdateAsync)} was cancelled before execution");
                cancellationToken.ThrowIfCancellationRequested();
            }

            var view = _eventDeserializer.Deserialize<TView>(await _cache.GetAsync(BuildKey(subject), cancellationToken));
            return view;
        }

        private string BuildKey(string key) => $"{_keyPrefix}:{key}";
    }
}
