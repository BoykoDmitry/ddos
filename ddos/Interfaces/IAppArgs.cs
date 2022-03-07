using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ddos.Interfaces
{
    public interface IAppArgs
    {
        int SecondsToRun { get; }
        bool UseProxy { get; }
        string TargetsFile { get; }
        int Threads { get; }
        int HttpTimeOut { get; }
        string Mode { get; }
    }
}
