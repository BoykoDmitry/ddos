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
    public abstract class BaseNuke : INuke
    {
        private readonly ILogger _logger;        
        private readonly IServiceProvider _serviceProvider;
        private readonly IAppArgs _appArgs;
        protected BaseNuke(ILogger logger,
             IServiceProvider serviceProvider,
             IAppArgs appArgs)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _appArgs = appArgs ?? throw new ArgumentNullException(nameof(appArgs));
        }

        protected ILogger Logger => _logger;
        protected IServiceProvider ServiceProvider => _serviceProvider;
        protected IAppArgs AppArgs => _appArgs;

        public abstract Task Boom(string destination, bool useProxy, CancellationToken cancellationToken);
       
        public abstract string WhoAmI();        
    }
}
