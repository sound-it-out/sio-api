using Microsoft.Extensions.DependencyInjection;

namespace SIO.Infrastructure
{
    public interface ISIOInfrastructureBuilder
    {
        IServiceCollection Services { get; }
    }
}
