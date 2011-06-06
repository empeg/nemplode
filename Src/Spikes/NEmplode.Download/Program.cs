using System;
using NEmplode.EmpegCar.Database;
using NEmplode.EmpegCar.Database.Sources;

namespace NEmplode.Download
{
    static class Program
    {
        static void Main(string[] args)
        {
            // TODO: Invent EmpegCarDatabaseSource.Create() factory method.
            string uriString = string.Format("http://{0}/", args[0]);
            Uri baseUri = new Uri(uriString);

            IEmpegCarDatabaseSource source = new HijackDatabaseSource(baseUri);
            var reader = new EmpegCarDatabaseReader(source);
            var database = reader.ReadDatabase();

            // TODO: For the purposes of this program (for now), we need a flat dump of everything on the player.
            foreach (var item in database.Items)
            {
                Console.WriteLine(item);
            }

            // TODO: Later, we'll need a heirarchical dump of everything on the player.

            Console.WriteLine(database);
        }
    }
}
