using Core.Entities.Chat;
using Core.Entities.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Data.ConfigData.Auth;
public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> modelBuilder)
    {
        modelBuilder
        .HasOne(c => c.Sender)
        .WithMany()
        .HasForeignKey(c => c.SenderId)
        .OnDelete(DeleteBehavior.NoAction);
        modelBuilder
        .HasOne(c => c.Recipient)
        .WithMany()
        .HasForeignKey(c => c.RecipientId)
        .OnDelete(DeleteBehavior.NoAction);
    }
}

