using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenEventSourcing.EntityFrameworkCore.EntityConfiguration;
using OpenEventSourcing.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Projections.Document
{
    internal class DocumentTypeConfiguration : IProjectionTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable(nameof(Document));
            builder.HasKey(s => s.Id);
            builder.Property(p => p.Id)
                   .ValueGeneratedNever();
            builder.Property(p => p.Data)
                   .HasJsonValueConversion();
        }
    }
}
