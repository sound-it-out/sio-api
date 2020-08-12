using System;
using Microsoft.Extensions.DependencyInjection;
using SIO.Domain.Projections.Document;
using SIO.Domain.Projections.User;
using SIO.Domain.Projections.UserDocument;

namespace SIO.Domain.Projections
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjections(this IServiceCollection source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            source.AddScoped<DocumentProjection>();
            source.AddScoped<UserProjection>();
            source.AddScoped<UserDocumentProjection>();

            source.AddHostedService<PollingProjector<DocumentProjection>>();
            source.AddHostedService<PollingProjector<UserProjection>>();
            source.AddHostedService<PollingProjector<UserDocumentProjection>>();

            return source;
        }
    }
}
