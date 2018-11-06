using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
namespace windowsservice
{
    class DemoWindowsService
    {
        private readonly string _path;
        private readonly ILogService _log;
        private readonly FaceApi _api;
        private IKernel kernel;
        private FileSystemWatcher watcher;
        public DemoWindowsService()
        {
            kernel = new StandardKernel(new DemoModule());
            var config = kernel.Get<IConfiguration>();
            _path = config.ImagesPath;
            _log = kernel.Get<ILogService>();
            _api=kernel.Get<FaceApi>();
        }
        public bool Start()
        {
            watcher = new FileSystemWatcher();
            watcher.Path=_path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.jpg";
            // Add event handlers.
            watcher.Changed += (sender,e) => SyncScan();
            watcher.Created += (sender, e) => SyncScan();
            watcher.Deleted += (sender, e) => SyncScan();
            watcher.Renamed += (sender, e) => SyncScan();

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            return true;
        }
        public bool Stop()
        {
            watcher.EnableRaisingEvents=false;
            watcher.Dispose();
            return true;
        }
        public void SyncScan()
        {
            Task.Run(()=>Scan()).Wait();
        }
        public async Task Scan()
        {
            var baseDir = new DirectoryInfo(_path);
            var images = baseDir.GetFiles("*.jpg");
            foreach (var image in images)
            {
                _log.Logger.Info("Found image {name}", image.Name);
                var jDataFile = new FileInfo(image.FullName.Replace(image.Extension, ".json"));
                if (jDataFile.Exists)
                {
                    _log.Logger.Info("Json data file found, skipping.");
                    continue;
                }
                _log.Logger.Info("Requesting face data from API");
                var json = await _api.MakeAnalysisRequest(image.FullName);
                _log.Logger.Info("Writing face data to json file {file}", jDataFile.Name);
                using (var jsonFile = new StreamWriter(jDataFile.FullName))
                {
                    jsonFile.WriteLine(json);
                }
            }
        }

    }
}
