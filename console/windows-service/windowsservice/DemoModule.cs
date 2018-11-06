using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace windowsservice
{
    class DemoModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogService>().To<LogService>().InSingletonScope();
            Bind<IConfiguration>().To<Configuration>().InSingletonScope();
            Bind<FaceApi>().ToSelf().InSingletonScope();
        }
    }
}
