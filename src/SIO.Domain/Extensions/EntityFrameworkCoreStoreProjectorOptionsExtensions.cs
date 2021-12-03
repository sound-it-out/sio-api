using SIO.Domain.Documents.Projections;
using SIO.Domain.Documents.Projections.Managers;
using SIO.Domain.TranslationOptions.Projections;
using SIO.Domain.TranslationOptions.Projections.Managers;
using SIO.Infrastructure.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Extensions
{
    public static class EntityFrameworkCoreStoreProjectorOptionsExtensions
    {
        public static void WithDomainProjections(this EntityFrameworkCoreStoreProjectorOptions options)
            => options.WithProjection<Document, DocumentProjectionManager>(o => o.Interval = 5000)
                .WithProjection<DocumentAudit, DocumentAuditProjectionManager>(o => o.Interval = 5000)
                .WithProjection<UserDocuments, UserDocumentsProjectionManager>(o => o.Interval = 5000)
                .WithProjection<TranslationOption, TranslationOptionProjectionManager>(o => o.Interval = 5000);
    }
}
