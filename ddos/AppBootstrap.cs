using ddos.App;
using ddos.Interfaces;
using ddos.Model;
using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ddos
{
    public static class AppBootstrap
    {
        public static IAppArgs GetAppArgs(string[] args)
        {
            var p = new FluentCommandLineParser<AppArgs>();

            p.Setup<int>(_ => _.SecondsToRun)
                        .As('s', "secondsToRun")
                        .SetDefault(120);

            p.Setup<bool>(_ => _.UseProxy)
            .As('p', "useProxy")
            .SetDefault(false);

            p.Setup<string>(_ => _.TargetsFile)
            .As('f', "targetsFile")
            .SetDefault("Targets.txt");

            p.Setup<int>(_ => _.Threads)
                        .As('t', "threads")
                        .SetDefault(100);

            p.Setup<int>(_ => _.HttpTimeOut)
            .As('o', "httpTimeOut")
            .SetDefault(10);

            p.Setup<string>(_ => _.Mode)
            .As('m', "mode")
            .SetDefault("Random");

            p.Parse(args);

            return p.Object;
        }

        public static IAppBuilder CreateAppBuilder(string[] args)
        {
            return new AppBuilder(GetAppArgs(args));
        }
    }
}
