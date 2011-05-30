namespace NEmplode.EmpegCar.Model.Database
{
    internal interface IEmpegDatabaseProvider
    {
        byte[] DownloadDatabase();
        byte[] DownloadTags();
        byte[] DownloadPlaylists();
    }
}