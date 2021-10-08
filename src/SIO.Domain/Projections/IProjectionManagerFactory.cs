using SIO.Infrastructure.Projections;

namespace SIO.Domain.Projections
{
    public interface IProjectionManagerFactory<TView>
        where TView : class, IProjection
    {
        IProjectionManager<TView> Create(params IProjectionWriter<TView>[] projectionWriters);
    }
}
