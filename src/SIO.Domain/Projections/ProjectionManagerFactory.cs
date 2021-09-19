using System;
using System.Collections.Generic;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Projections
{
    public class ProjectionManagerFactory<TView> : IProjectionManagerFactory<TView>
        where TView : class, IProjection
    {
        private readonly Func<IEnumerable<IProjectionWriter<TView>>, IProjectionManager<TView>> _factoryFunc;

        public ProjectionManagerFactory(Func<IEnumerable<IProjectionWriter<TView>>, IProjectionManager<TView>> factoryFunc)
        {
            if (factoryFunc == null)
                throw new ArgumentNullException(nameof(factoryFunc));

            _factoryFunc = factoryFunc;
        }

        public IProjectionManager<TView> Create(params IProjectionWriter<TView>[] projectionWriters) => _factoryFunc(projectionWriters);
    }
}
