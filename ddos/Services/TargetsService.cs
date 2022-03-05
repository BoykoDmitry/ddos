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
    public class TargetsService : ITargetsService
    {
        private object _lockObject = new object();
        private readonly ILogger _logger;
        private readonly IAppArgs _appArgs;
        public TargetsService(IAppArgs appArgs, ILogger<TargetsService> logger)
        {
            _appArgs = appArgs ?? throw new ArgumentNullException(nameof(appArgs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        private IEnumerable<string> _targets;
        private IEnumerable<string> Targets
        {
            get {
                lock (_lockObject)
                    _targets = _targets ?? File.ReadAllLines(Environment.CurrentDirectory.ToString() + $"\\{_appArgs.TargetsFile}");

                return _targets; 
            }
        }

        public string GetOne()
        {
            Random random = new Random();
            return Targets.ToArray()[random.Next(Targets.Count() - 1)];
        }

        public IEnumerable<string> GetTargets()
        {
            return Targets;
        }
    }
}
