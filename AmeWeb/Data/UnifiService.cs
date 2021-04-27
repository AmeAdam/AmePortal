using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UnifiApi;
using UnifiApi.Models;

namespace AmeWeb.Data
{
    public class UnifiService
    {
        private readonly IConfiguration _config;

        public UnifiService(IConfiguration config)
        {
            _config = config;
        }

        private async Task<Client> GetClient()
        {
            var client = new Client(_config["Unifi:Address"], null, true);
            await client.LoginAsync(_config["Unifi:User"], _config["Unifi:Pass"]);
            return client;
        }

        public async Task<List<ClientList>> GetAllClients()
        {
            using var unifiClient = await GetClient();
            var result = await unifiClient.ListAllClientsAsync();
            return result.Data;
        }

        public async Task BlockClient(string mac)
        {
            using var unifiClient = await GetClient();
            var result = await unifiClient.BlockClientAsync(mac.Replace("-",":"));
        }

        public async Task UnblockClient(string mac)
        {
            using var unifiClient = await GetClient();
            var result = await unifiClient.UnblockClientAsync(mac.Replace("-", ":"));
        }
    }
}
