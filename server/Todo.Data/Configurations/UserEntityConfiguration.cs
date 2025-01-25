using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities;

namespace Todo.Data.Configurations;

/// <summary>
///     The entity configuration for the User entity.
/// </summary>
public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    ///     Configures the entity of type Task.
    /// </summary>
    /// <param name="builder"> The builder to be used to configure the entity. </param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.HasIndex(user => user.Id);

        builder.Property(user => user.Id)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(user => user.Email);

        builder.Property(user => user.UserName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(user => user.PhoneNumber)
            .IsRequired(false)
            .HasMaxLength(17);

        builder.Property(user => user.FirstName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(user => user.LastName)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasMany(user => user.Lists)
            .WithOne(list => list.User)
            .HasForeignKey(list => list.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}