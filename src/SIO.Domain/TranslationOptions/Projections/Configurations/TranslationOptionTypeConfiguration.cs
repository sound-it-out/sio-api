using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIO.Infrastructure.EntityFrameworkCore.EntityConfiguration;

namespace SIO.Domain.TranslationOptions.Projections.Configurations
{
    internal class TranslationOptionTypeConfiguration : IProjectionTypeConfiguration<TranslationOption>
    {
        public void Configure(EntityTypeBuilder<TranslationOption> builder)
        {
            builder.ToTable(nameof(TranslationOption));
            builder.HasKey(to => to.Id);
            builder.Property(to => to.Id)
                   .ValueGeneratedNever();
        }
    }
}
