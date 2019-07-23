using System;

namespace SIO.Abstraction.Commands
{
    public interface ICommand
    {
        Guid AggregateId { get; }
        Guid? CorrelationId { get; }
    }
}
