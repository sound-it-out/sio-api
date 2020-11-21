using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenEventSourcing.EntityFrameworkCore.EntityConfiguration;
using OpenEventSourcing.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Projections.UsersDocuments
{
    internal class UserDocumentTypeConfiguration : IProjectionTypeConfiguration<UserDocument>
    {
        public void Configure(EntityTypeBuilder<UserDocument> builder)
        {
            builder.ToTable(nameof(UserDocument));
            builder.HasKey(s => s.DocumentId);
            builder.Property(p => p.DocumentId)
                   .ValueGeneratedNever();
            builder.Property(p => p.UserId)
                   .ValueGeneratedNever();
            builder.Property(p => p.Data)
                   .HasJsonValueConversion();
        }
    }
}
