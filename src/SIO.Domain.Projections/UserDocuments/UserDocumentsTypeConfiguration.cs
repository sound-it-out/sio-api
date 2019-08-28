using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenEventSourcing.EntityFrameworkCore.EntityConfiguration;
using OpenEventSourcing.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Projections.UserDocuments
{
    internal class UserDocumentsTypeConfiguration : IProjectionTypeConfiguration<UserDocuments>
    {
        public void Configure(EntityTypeBuilder<UserDocuments> builder)
        {
            builder.ToTable(nameof(UserDocuments));
            builder.HasKey(s => s.UserId);
            builder.Property(p => p.UserId)
                   .ValueGeneratedNever();
            builder.Property(p => p.Data)
                   .HasJsonValueConversion();
        }
    }
}
