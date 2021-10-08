using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIO.Infrastructure.EntityFrameworkCore.EntityConfiguration;

namespace SIO.Domain.Documents.Projections.Configurations
{
    internal class DocumentTypeConfiguration : IProjectionTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable(nameof(Document));
            builder.HasKey(s => s.Id);
            builder.Property(p => p.Id)
                   .ValueGeneratedNever();
        }
    }
}
