using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ddos.Interfaces
{
    [Flags]
    public enum ExitCode : int
    {
        Success = 0,
        Fail = -1
    }

    public interface IApp
    {
        Task<ExitCode> Run(CancellationToken cancellationToken);

        IConfiguration Configuration { get; }
        IServiceProvider ServiceProvider { get; }

    }
}
