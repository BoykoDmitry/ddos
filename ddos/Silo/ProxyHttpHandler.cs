﻿using ddos.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ddos.Silo
{
    public class ProxyHttpHandler : HttpClientHandler
    {
        private readonly IProxyResolverService _proxyResolverService;
        private readonly ILogger _logger;
        public ProxyHttpHandler(IProxyResolverService proxyResolverService, ILogger<ProxyHttpHandler> logger)
        {
            _proxyResolverService = proxyResolverService ?? throw new ArgumentNullException(nameof(proxyResolverService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var proxyAddress = _proxyResolverService.GetProxy();
            _logger.LogTrace($"Using proxy: {proxyAddress}");
            Proxy = new WebProxy(proxyAddress);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
