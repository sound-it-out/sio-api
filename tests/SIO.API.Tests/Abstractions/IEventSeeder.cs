using System.Threading.Tasks;
using OpenEventSourcing.Events;

namespace SIO.API.Tests.Abstractions
{
    public interface IEventSeeder
    {
        Task SeedAsync(params IEvent[] events);
    }
}
