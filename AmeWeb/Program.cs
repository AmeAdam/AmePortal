using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnifiApi;

namespace AmeDhcp
{
    //https://github.com/schwoi/UnifiClient
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //{mac: "d8:f1:5b:8c:56:e9", cmd: "kick-sta"} - restart clienta
            //using (var unifiClient = new Client("https://localhost:8443", null, true))
            //{
            //    await unifiClient.LoginAsync(config["Unifi:User"], config["Unifi:Pass"]);
            //    var result = await unifiClient.ListAllClientsAsync();
            //}

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
