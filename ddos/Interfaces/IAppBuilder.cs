using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ddos.Interfaces
{
    public interface IAppBuilder
    {
        IApp Build();
        IAppBuilder ConfigureServices(Action<IServiceCollection> configureServices);
        IAppBuilder ConfigureServices(Action<IServiceCollection, IAppArgs> configureServices);
        IAppBuilder ConfigureLogging(Action<ILoggingBuilder> configureDelegate);
        IAppBuilder ConfigureLogging(Action<IServiceCollection, IConfiguration> configureDelegate);
        IAppBuilder ConfigureLogging(Action<IServiceCollection, IAppArgs, IConfiguration> configureDelegate);
        IAppBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder, IAppArgs> configureDelegate);
        IAppBuilder ConfigureHttpClients();
        IAppBuilder SetUpNuke();
    }
}
