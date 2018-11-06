using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace windowsservice
{
    class Program
    {
        static void Main(string[] args)
        {
            var rc=HostFactory.Run(config=>
            {
                config.UseNLog();
                config.Service<DemoWindowsService>(s=>
                {
                    s.ConstructUsing(srv=>new DemoWindowsService());
                    s.WhenStarted((service,control)=>service.Start());
                    s.WhenStopped((service,control)=>service.Stop());
                });
                config.SetDescription("Demo windows service for watching files and using Azure Face API");
                config.SetDisplayName("Demo service");
                config.SetServiceName("DemoService");
                config.StartAutomaticallyDelayed();
            });
            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
