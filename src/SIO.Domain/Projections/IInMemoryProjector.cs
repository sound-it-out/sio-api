using System;
using System.Threading;
using System.Threading.Tasks;
using SIO.Infrastructure.Events;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Projections
{
    public interface IInMemoryProjector<TView>
        where TView: IProjection
    {
        Task<TView> ProjectAsync(string subject, CancellationToken cancellationToken = default);
        Task<TView> ProjectAsync(string subject, DateTimeOffset timestamp, CancellationToken cancellationToken = default);
        Task<TView> ApplyAsync(TView view, IEvent @event, CancellationToken cancellationToken = default);
    }
}
