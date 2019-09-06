using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenEventSourcing.EntityFrameworkCore.EntityConfiguration;
using OpenEventSourcing.EntityFrameworkCore.Extensions;

namespace SIO.Domain.Projections.User
{
    internal class UserTypeConfiguration : IProjectionTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(s => s.Id);
            builder.Property(p => p.Id)
                   .ValueGeneratedNever();
            builder.Property(p => p.Data)
                   .HasJsonValueConversion();
        }
    }
}
