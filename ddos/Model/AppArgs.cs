using ddos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ddos.Model
{
    public class AppArgs : IAppArgs
    {
        public int SecondsToRun { get; set; } = 120;
        public bool UseProxy { get; set; } = false;
        public string TargetsFile { get; set; } = "Targets.txt";
        public int Threads { get; set; } = 100;
        public int HttpTimeOut { get; set; } = 10;
        public string Mode { get; set; } = "Http";
    }
}
