namespace NEmplode.EmpegCar.Database.Sources
{
    public interface IEmpegCarDatabaseSource
    {
        byte[] DownloadDatabase();
        byte[] DownloadTags();
        byte[] DownloadPlaylists();
    }
}