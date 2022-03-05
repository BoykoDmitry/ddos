using ddos.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ddos.Services
{
    public class ProxyResolverService : IProxyResolverService
    {
        private object _lockObject = new object();
        private readonly ILogger _logger;
        private readonly IAppArgs _appArgs;
        public ProxyResolverService(IAppArgs appArgs, ILogger<ProxyResolverService> logger)
        {
            _appArgs = appArgs ?? throw new ArgumentNullException(nameof(appArgs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private IEnumerable<string> _proxies;
        public string GetProxy()
        {
            Random random = new Random();
            lock (_lockObject)
                _proxies = _proxies ?? File.ReadAllLines(Environment.CurrentDirectory.ToString() + "\\ProxyList.txt");

            if (!_proxies.Any())
                throw new Exception("Proxy list is empty!");

            return _proxies.ToArray()[random.Next(0, _proxies.Count() - 1)];
        }
    }
}
