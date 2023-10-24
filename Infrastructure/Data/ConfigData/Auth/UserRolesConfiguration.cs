using Core.Entities.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Users.API.Data;
public class UserRolesConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        builder.ToTable("UsersRoles");

        // relation many to many
        builder.HasOne(ur => ur.Role)
         .WithMany(r => r.UserRole).HasForeignKey(ur => ur.RoleId)
         .IsRequired();
        builder.HasOne(ur => ur.User)
        .WithMany(r => r.UserRole)
        .HasForeignKey(ur => ur.UserId)
        .IsRequired();
        builder.HasQueryFilter(s => !s.Role.IsDeleted);
    }
}

