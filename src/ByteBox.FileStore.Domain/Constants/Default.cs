namespace ByteBox.FileStore.Domain.Constants;

public static class Default
{
    public static class User
    {
        public static readonly Guid UserId = Guid.Parse("9A18B0B3-C515-412D-BEF1-B609450DE4C9");
        public const string UserName = "Default User";
        public const string Email = "default.user@bytebox.com";
    }

    public static class Folder
    {
        public const string RootFolderName = "Root";
    }

    public static readonly DateTime NextBillDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}
