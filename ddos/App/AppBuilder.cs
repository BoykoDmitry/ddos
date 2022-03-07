using ddos.Interfaces;
using ddos.Services;
using ddos.Silo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ddos.App
{
    public class AppBuilder : IAppBuilder
    {
        IConfigurationRoot _configuration;
        IServiceCollection _services;
        IAppArgs _appArgs;
        IServiceProvider _provider;
        public AppBuilder(IAppArgs appArgs)
        {
            _appArgs = appArgs;
            _services = new ServiceCollection();

        }

        public IApp Build()
        {
            _services.AddSingleton<IApp,NukeApp>();
            _services.AddSingleton<IAppArgs>(_appArgs);
            _services.AddSingleton<ITools, Tools>();

            _provider = _services
                            .BuildServiceProvider();

            return _provider
                            .GetService<IApp>();
        }

        public IAppBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder, IAppArgs> configureDelegate)
        {
            var cnf = new ConfigurationBuilder();
            configureDelegate?.Invoke(cnf, _appArgs);

            _configuration = cnf.Build();

            _services.AddSingleton<IConfiguration>(_configuration);

            return this;
        }

        public IAppBuilder ConfigureHttpClients()
        {
            _services.AddHttpClient("HttpClient", _=>_.Timeout = TimeSpan.FromSeconds(_appArgs.HttpTimeOut));                              
            _services.AddTransient<ProxyHttpHandler>();

            return this;
        }

        public IAppBuilder ConfigureLogging(Action<ILoggingBuilder> configureDelegate)
        {
            _services.AddLogging(configureDelegate);

            return this;
        }

        public IAppBuilder ConfigureLogging(Action<IServiceCollection, IAppArgs, IConfiguration> configureDelegate)
        {
            if (_configuration == null)
                throw new Exception("Configuration must be configured first!");

            configureDelegate?.Invoke(_services, _appArgs, _configuration);
            return this;
        }

        public IAppBuilder ConfigureLogging(Action<IServiceCollection, IConfiguration> configureServices)
        {
            if (_configuration == null)
                throw new Exception("Configuration must be configured first!");

            configureServices?.Invoke(_services, _configuration);

            return this;
        }

        public IAppBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            configureServices?.Invoke(_services);

            return this;
        }

        public IAppBuilder ConfigureServices(Action<IServiceCollection, IAppArgs> configureServices)
        {
            configureServices?.Invoke(_services, _appArgs);

            return this;
        }

        public IAppBuilder SetUpNuke()
        {
            _services.AddTransient<INuke,UdpNuke>();
            _services.AddTransient<INuke, HeavyTcpNuke>();
            _services.AddTransient<INuke, TcpEzNuke>();
            _services.AddTransient<INuke, HttpNuke>();
            _services.AddSingleton<IProxyResolverService, ProxyResolverService>();
            _services.AddSingleton<ITargetsService, TargetsService>();

            return this;
        }
    }
}
