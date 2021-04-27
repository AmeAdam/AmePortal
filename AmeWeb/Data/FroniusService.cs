using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FroniusSolarClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AmeWeb.Data
{
    public class FroniusService
    {
        private readonly ILogger _logger;
        private readonly string _froniusUrl;

        public FroniusService(IConfiguration config, ILogger<FroniusService> logger)
        {
            _logger = logger;
            _froniusUrl = config["FroniusUrl"];
        }

        public async Task<FroniusData> GetSolarData()
        {
            var solarClient = new SolarClient(_froniusUrl, 1, _logger);
            try
            {
                var response = solarClient.GetCommonInverterData();
                
                return new FroniusData
                {
                    Power = response?.Body.Data.AcPower.Value ?? 0,
                    DayPower = response?.Body.Data.CurrentDayEnergy.Value / 1000 ?? 0,
                    TotalPower = response?.Body.Data.TotalEnergy.Value / 1000 ?? 0
                };
            }
            catch (Exception)
            {
                return new FroniusData();
            }
        }
    }
}
