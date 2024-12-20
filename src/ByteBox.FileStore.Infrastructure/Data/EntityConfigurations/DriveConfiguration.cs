using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteBox.FileStore.Infrastructure.Data.EntityConfigurations;

public class DriveConfiguration : IEntityTypeConfiguration<Drive>
{
    public void Configure(EntityTypeBuilder<Drive> builder)
    {
        builder.ToTable(Tables.Drives)
            .HasKey(d => d.DriveId);

        builder
            .HasOne(d => d.Owner)
            .WithOne()
            .HasForeignKey<Drive>(d => d.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(d => d.IsDeleted)
            .HasDefaultValue(false);

        builder
            .HasData(GetSeedDrives());
    }

    private static List<Drive> GetSeedDrives()
    {
        return new List<Drive>
        {
            new Drive
            {
                DriveId = Default.User.UserId,
                PurchasedStorageInMb = 1024,
                UsedStorageInMb = 0,
                NextBillDate = Default.NextBillDate,
                OwnerId = Default.User.UserId
            }
        };
    }
}
