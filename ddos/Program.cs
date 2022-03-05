using ddos.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ddos
{
    internal class Program
    {
        // very default logger, which will be able to write logs in console before actual logger is configured 
        static Serilog.ILogger Logger => new LoggerConfiguration().WriteTo.Console().CreateLogger();
        static IApp app = null;
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                app = BuildApp(args);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Fatal error during bootstrapping the app");
                Environment.Exit((int)ExitCode.Fail);
            }

            var res = await app.Run(cts.Token);

            Environment.Exit((int)res);

        }

        static IApp BuildApp(string[] args) =>
            AppBootstrap.CreateAppBuilder(args)
                        .ConfigureAppConfiguration((conf, a) => conf.SetBasePath(Directory.GetCurrentDirectory())
                                                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
                        .ConfigureLogging((l, c) => l.AddLogging(p => p.AddSerilog(new LoggerConfiguration()
                                                                           .ReadFrom
                                                                           .Configuration(c)
                                                                           .CreateLogger())
                                                      )
                               )
                        .ConfigureHttpClients()
                        .SetUpNuke()
                        .Build();   
    }
}
