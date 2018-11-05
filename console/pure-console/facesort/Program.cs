using System.Threading.Tasks;
using Ninject;

namespace FaceSort
{
    internal static class Program
    {
        private static void Main()
        {
            using (var kernel = new KernelConfiguration(
                    new FaceSortModule())
                .BuildReadonlyKernel())
            {
                var log = kernel.Get<ILogService>();
                log.Logger.Info("Starting up main program");
                var imageService = kernel.Get<ImageIndexingService>();
                Task.Run(()=>imageService.Scan()).Wait();
                log.Logger.Info("Execution done, shutdown.");
            }
        }
    }
}