﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SIO.Domain.Audits.Projections;
using SIO.Domain.Documents.CommandHandlers;
using SIO.Domain.Documents.Commands;
using SIO.Domain.Documents.Projections;
using SIO.Domain.Documents.Projections.Managers;
using SIO.Domain.Processes;
using SIO.Infrastructure.Commands;
using SIO.Infrastructure.Projections;

namespace SIO.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            return services.AddDocuments();
        }

        public static IServiceCollection AddDocuments(this IServiceCollection services)
        {
            // Projections
            services.AddScoped<IProjectionManager<DocumentAudit>, DocumentAuditProjectionManager>();
            services.AddScoped<IProjectionManager<Document>, DocumentProjectionManager>();
            services.AddScoped<IProjectionManager<UserDocuments>, UserDocumentsProjectionManager>();

            services.AddSingleton<IProjector<DocumentAudit>, StoreProjector<DocumentAudit>>();
            services.AddSingleton<IProjector<Document>, StoreProjector<Document>>();
            services.AddSingleton<IProjector<UserDocuments>, StoreProjector<UserDocuments>>();
            services.AddSingleton(sp => DocumentAuditProjectionManagerFactory(sp));
            services.AddSingleton(sp => DocumentProjectionManagerFactory(sp));
            services.AddSingleton(sp => UserDocumentsProjectionManagerFactory(sp));

            //Register hosted services from singleton to ensure that we can stop and start on demand;
            //services.AddHostedService(sp => sp.GetRequiredService<IProjector<DocumentAudit>>());
            //services.AddHostedService(sp => sp.GetRequiredService<IProjector<Document>>());
            //services.AddHostedService(sp => sp.GetRequiredService<IProjector<UserDocuments>>());

            //Commands
            services.AddScoped<ICommandHandler<UploadDocumentCommand>, UploadDocumentCommandHandler>();

            return services;
        }

        private static Func<IEnumerable<IProjectionWriter<UserDocuments>>, IProjectionManager<UserDocuments>> UserDocumentsProjectionManagerFactory(IServiceProvider serviceProvider)
        {
            return new Func<IEnumerable<IProjectionWriter<UserDocuments>>, IProjectionManager<UserDocuments>>(writers =>
            {
                return new UserDocumentsProjectionManager(serviceProvider.GetRequiredService<ILogger<ProjectionManager<UserDocuments>>>(), writers);
            });
        }

        private static Func<IEnumerable<IProjectionWriter<Document>>, IProjectionManager<Document>> DocumentProjectionManagerFactory(IServiceProvider serviceProvider)
        {
            return new Func<IEnumerable<IProjectionWriter<Document>>, IProjectionManager<Document>>(writers =>
            {
                return new DocumentProjectionManager(serviceProvider.GetRequiredService<ILogger<ProjectionManager<Document>>>(), writers);
            });
        }

        private static Func<IEnumerable<IProjectionWriter<DocumentAudit>>, IProjectionManager<DocumentAudit>> DocumentAuditProjectionManagerFactory(IServiceProvider serviceProvider)
        {
            return new Func<IEnumerable<IProjectionWriter<DocumentAudit>>, IProjectionManager<DocumentAudit>>(writers =>
            {
                var tempWriters = new IProjectionWriter<Audit>[writers.Count()];

                var i = 0;

                foreach(var temp in writers)
                    tempWriters[i] = (IProjectionWriter<Audit>)temp;

                return new DocumentAuditProjectionManager(serviceProvider.GetRequiredService<ILogger<ProjectionManager<DocumentAudit>>>(), tempWriters);
            });
        }
    }
}
