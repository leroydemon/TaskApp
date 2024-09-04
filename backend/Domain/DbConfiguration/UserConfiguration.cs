using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.DbConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Identity have default setting for property and we can configure here or in IdentityOptions
            builder
                .Property(x => x.UserName)
                .IsRequired();

            builder
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder
                .Property(x => x.PasswordHash)
                .IsRequired();
        }
    }
}
