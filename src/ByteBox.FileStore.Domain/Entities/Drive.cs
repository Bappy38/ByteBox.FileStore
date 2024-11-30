using ByteBox.FileStore.Domain.Interfaces;

namespace ByteBox.FileStore.Domain.Entities;

// TODO: Application will create the drive when an user signed up
// TODO: Application will also create a root folder named "Root" and folderId will be the same as driveId.
// TODO: For each environment (e.g. dev, test, live) there will be a dedicated s3 bucket. Each user's resources will be stored in a separate folder in that bucket so that we can easily cleanup whenever a user delete his profile from the app.
public class Drive : ISoftDeletable
{
    public Guid DriveId { get; set; }
    public double PurchasedStorageInMb { get; set; }
    public double UsedStorageInMb { get; set; }
    public DateTime NextBillDate { get; set; }

    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    public bool IsDeleted { get; set; }
}
