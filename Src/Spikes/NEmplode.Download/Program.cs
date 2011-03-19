using System;

namespace NEmplode.Download
{
    static class Program
    {
        static void Main(string[] args)
        {
            Uri baseUri = new Uri("http://10.0.0.99/");

            var reader = new HijackDatabaseReader(baseUri);
            var database = reader.ReadDatabase();
        }
    }
}
