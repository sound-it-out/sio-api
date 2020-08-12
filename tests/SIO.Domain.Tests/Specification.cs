using System;
using System.Collections.Generic;
using OpenEventSourcing.Domain;
using OpenEventSourcing.Events;
using SIO.Tests.Infrastructure;

namespace SIO.Tests.Unit.Domain
{
    public abstract class Specification<TAggregateRoot, TState>
            where TAggregateRoot : Aggregate<TState>
            where TState : IAggregateState, new()
    {
        private ExceptionMode _exceptionMode;

        protected TAggregateRoot Aggregate { get; }
        protected Exception Exception { get; }

        protected abstract IEnumerable<IEvent> Given();
        protected abstract void When();
        protected void RecordExceptions()
        {
            _exceptionMode = ExceptionMode.Record;
        }


        protected Specification()
        {
            Aggregate = (TAggregateRoot)Activator.CreateInstance(typeof(TAggregateRoot), new object[] { new TState() });

            var events = Given();

            try
            {
                Aggregate.FromHistory(events);

                When();
            }
            catch (Exception ex) when (_exceptionMode == ExceptionMode.Record)
            {
                Exception = ex;
            }
        }
    }
}
