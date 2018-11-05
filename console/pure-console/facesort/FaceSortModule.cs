using Ninject.Modules;

namespace FaceSort
{
    public class FaceSortModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogService>().To<LogService>().InSingletonScope();
            Bind<IConfiguration>().To<Configuration>().InSingletonScope();
            Bind<ImageIndexingService>().ToSelf().InSingletonScope();
            Bind<FaceApi>().ToSelf().InSingletonScope();
        }
    }
}