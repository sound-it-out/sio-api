using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenEventSourcing.EntityFrameworkCore.EntityConfiguration;
using OpenEventSourcing.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Projections.UserDocument
{
    internal class UserDocumentTypeConfiguration : IProjectionTypeConfiguration<UserDocument>
    {
        public void Configure(EntityTypeBuilder<UserDocument> builder)
        {
            builder.ToTable(nameof(UserDocument));
            builder.HasKey(s => new { s.DocumentId, s.UserId});
            builder.Property(p => p.DocumentId)
                   .ValueGeneratedNever();
            builder.Property(p => p.UserId)
                   .ValueGeneratedNever();
            builder.Property(p => p.Data)
                   .HasJsonValueConversion();
        }
    }
}
