using System.Net;
using System.Text;

namespace NEmplode.EmpegCar.Discovery
{
    public class EmpegLocator
    {
        public IPEndPoint EndPoint { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return string.Format("{0} on {1} (Id {2})", Name, EndPoint.Address, Id);
        }

        private EmpegLocator(IPEndPoint endPoint, string name, string id)
        {
            EndPoint = endPoint;
            Name = name;
            Id = id;
        }

        public static EmpegLocator Create(IPEndPoint ep, byte[] bytes)
        {
            var responseText = Encoding.UTF8.GetString(bytes);
            var responseParts = responseText.Split('\n');
            string name = null;
            string id = null;
            foreach (var responsePart in responseParts)
            {
                var keyAndValue = responsePart.Split('=');
                if (keyAndValue[0] == "name")
                    name = keyAndValue[1];
                else if (keyAndValue[0] == "id")
                    id = keyAndValue[1];
            }

            return new EmpegLocator(ep, name, id);
        }
    }
}
