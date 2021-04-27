using System.Net;

namespace AmeWeb.Data
{
    public class DhcpEntry
    {
        public string Mac { get; set; }
        public string Hostname { get; set; }
        public string ClientIp { get; set; }
        public IPAddress ClientIpAddress { get; set; }
        public string Gateway { get; set; }
    }
}
