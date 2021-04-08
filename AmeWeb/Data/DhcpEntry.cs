using System.Net;

namespace AmeDhcp.Data
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
