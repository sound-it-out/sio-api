using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIO.Infrastructure.EntityFrameworkCore.EntityConfiguration;

namespace SIO.Domain.Audits.Projections.Configurations
{
    internal class UserDocumentsTypeConfiguration : IProjectionTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable(nameof(Audit));
            builder.HasKey(ud => ud.Subject);
            builder.Property(ud => ud.Subject)
                   .ValueGeneratedNever();

            builder.OwnsMany(ud => ud.Events,b => {
                b.WithOwner().HasForeignKey("AuditId");
                b.HasKey(ae => ae.Id);
                b.Property(ae => ae.Id)
                    .ValueGeneratedNever();
            });
        }
    }
}
