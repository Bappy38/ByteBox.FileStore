namespace ByteBox.FileStore.Domain.Entities;

// TODO: Application will create the drive when an user signed up
// TODO: Application will also create a root folder named "Root" and folderId will be the same as driveId.
public class Drive
{
    public Guid DriveId { get; set; }
    public double PurchasedStorageInMb { get; set; }
    public double UsedStorageInMb { get; set; }
    public DateTime NextBillDate { get; set; }

    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
}
