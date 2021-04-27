using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using IniParser.Parser;
using Microsoft.Extensions.Configuration;

namespace AmeWeb.Data
{
    public class DhcpService
    {
        private readonly IConfiguration config;
        private readonly FileIniDataParser parser;

        public DhcpService(IConfiguration config)
        {
            this.config = config;
            var parserConfig = new IniDataParser();
            parserConfig.Configuration.AssigmentSpacer = "";
            parser = new FileIniDataParser(parserConfig);
        }

        private string ConfigFile => config["DhcpConfigFile"];
        private IniData DhcpData() => parser.ReadFile(ConfigFile);

        public Task<DhcpEntry[]> GetEntries()
        {
            return Task.FromResult(DhcpData().Sections.Where(s => PhysicalAddress.TryParse(s.SectionName, out _)).Select(
                s => new DhcpEntry
                {
                    Mac = s.SectionName,
                    Hostname = s.Keys["Hostname"] ?? "Unknown",
                    ClientIp = s.Keys["IPADDR"],
                    ClientIpAddress = IPAddress.Parse(s.Keys["IPADDR"]),
                    Gateway = s.Keys["ROUTER_0"]
                })
                .ToArray());
        }
        public Task<DhcpEntry> GetEntry(string mac)
        {
            try
            {
                return Task.FromResult(DhcpData().Sections.Where(s => s.SectionName == mac).Select(
                    s => new DhcpEntry
                    {
                        Mac = s.SectionName,
                        Hostname = s.Keys["Hostname"],
                        ClientIp = s.Keys["IPADDR"],
                        ClientIpAddress = IPAddress.Parse(s.Keys["IPADDR"]),
                        Gateway = s.Keys["ROUTER_0"]
                    })
                    .FirstOrDefault());
            }
            catch(Exception ex)
            {
                return Task.FromResult(new DhcpEntry { Hostname = ex.Message});
            }
        }

        public void Update(string mac, string gateway)
        {
            IniData data = DhcpData();
            var clientSection = data.Sections[mac];

            if (gateway == "")
            {
                clientSection.RemoveKey("ROUTER_0");
                clientSection.RemoveKey("DNS_0");
            }
            else
            {
                clientSection["ROUTER_0"] = gateway;
                clientSection["DNS_0"] = gateway;
            }
            parser.WriteFile(ConfigFile, data, new UTF8Encoding(false));
        }
        public void Update(DhcpEntry entry)
        {
            IniData data = DhcpData();
            var clientSection = data.Sections[entry.Mac];

            clientSection["Hostname"] = entry.Hostname;
            clientSection["IPADDR"] = entry.ClientIp;

            if (entry.Gateway == "")
            {
                clientSection.RemoveKey("ROUTER_0");
                clientSection.RemoveKey("DNS_0");
            }
            else
            {
                clientSection["ROUTER_0"] = entry.Gateway;
                clientSection["DNS_0"] = entry.Gateway;
            }
            parser.WriteFile(ConfigFile, data, new UTF8Encoding(false));
        }

        public void Delete(string mac)
        {
            IniData data = DhcpData();
            data.Sections.RemoveSection(mac);
            parser.WriteFile(ConfigFile, data, new UTF8Encoding(false));
        }
    }
}
