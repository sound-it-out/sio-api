using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIO.Infrastructure.EntityFrameworkCore.EntityConfiguration;

namespace SIO.Domain.Documents.Projections.Configurations
{
    internal class UserDocumentsTypeConfiguration : IProjectionTypeConfiguration<UserDocuments>
    {
        public void Configure(EntityTypeBuilder<UserDocuments> builder)
        {
            builder.ToTable(nameof(UserDocuments));
            builder.HasKey(ud => ud.Id);
            builder.Property(ud => ud.Id)
                   .ValueGeneratedNever();

            builder.OwnsMany(uds => uds.Documents, b => {
                b.WithOwner().HasForeignKey("ParentId");
                b.HasKey(ud => ud.DocumentId);
                b.Property(ud => ud.DocumentId)
                    .ValueGeneratedNever();
            });
        }
    }
}
