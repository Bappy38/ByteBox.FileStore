namespace ByteBox.FileStore.Domain.BackgroundJobs;

public interface IDeleteTrashFilesJob
{
    Task ExecuteAsync();
}
