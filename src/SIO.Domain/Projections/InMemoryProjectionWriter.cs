using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Projections
{
    internal sealed class InMemoryProjectionWriter<TView> : IProjectionWriter<TView>
        where TView : class, IProjection
    {
        private readonly ConcurrentDictionary<string, TView> _cache;

        private InMemoryProjectionWriter()
        {
            _cache = new();
        }

        public static async Task<InMemoryProjectionWriter<TView>> BuildAsync(string subject, TView view, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var writer = new InMemoryProjectionWriter<TView>();
            await writer.LoadViewAsync(subject, view, cancellationToken);
            return writer;
        }

        public static Task<InMemoryProjectionWriter<TView>> BuildAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(new InMemoryProjectionWriter<TView>());
        }

        public async Task LoadViewAsync(string subject, TView view, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            if (_cache.ContainsKey(subject))
                await RemoveAsync(subject, cancellationToken);

            await AddAsync(subject, () => view, cancellationToken);
        }

        public Task<TView> AddAsync(string subject, Func<TView> add, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var view = add();
            _cache.TryAdd(subject, view);
            return Task.FromResult(view);
        }

        public Task RemoveAsync(string subject, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            _cache.TryRemove(subject, out _);

            return Task.CompletedTask;
        }

        public Task<TView> UpdateAsync(string subject, Func<TView, TView> update, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            if (!_cache.TryGetValue(subject, out var oldView))
                throw new InvalidOperationException("");

            var view = oldView;

            update(view);

            _cache.TryUpdate(subject, view, oldView);

            return Task.FromResult(view);
        }

        public async Task<TView> UpdateAsync(string subject, Action<TView> update, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            var view = await UpdateAsync(subject, view =>
            {
                update(view);
            });

            return view;
        }

        public Task ResetAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            _cache.Clear();

            return Task.CompletedTask;
        }

        public Task<TView> RetrieveAsync(string subject, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                cancellationToken.ThrowIfCancellationRequested();

            if (!_cache.TryGetValue(subject, out var view))
                throw new InvalidOperationException("");

            return Task.FromResult(view);
        }
    }
}
