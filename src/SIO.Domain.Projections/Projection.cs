using System;
using OpenEventSourcing.Projections;

namespace SIO.Domain.Projections
{
    public abstract class Projection<TView> : Projection
        where TView : class
    {
        protected readonly IProjectionWriter<TView> _writer;

        public Projection(IProjectionWriter<TView> writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }
    }
}
