using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FaceSort
{
    public class ImageIndexingService
    {
        private readonly string _path;
        private readonly ILogService _log;
        private readonly FaceApi _api;
        public ImageIndexingService(
            IConfiguration config,
            ILogService logger,
            FaceApi faceApi
            )
        {
            _path = config.ImagesPath;
            _log = logger;
            _api = faceApi;
        }

        public async Task Scan()
        {
            var baseDir = new DirectoryInfo(_path);
            var images = baseDir.GetFiles("*.jpg");
            foreach (var image in images)
            {
                _log.Logger.Info("Found image {name}",image.Name);
                var jDataFile = new FileInfo(image.FullName.Replace(image.Extension, ".json"));
                if (jDataFile.Exists)
                {
                    _log.Logger.Info("Json data file found, skipping.");
                    continue;
                }
                _log.Logger.Info("Requesting face data from API");
                var json = await _api.MakeAnalysisRequest(image.FullName);
                _log.Logger.Info("Writing face data to json file {file}",jDataFile.Name);
                using (var jsonFile = new StreamWriter(jDataFile.FullName))
                {
                    jsonFile.WriteLine(json);    
                }
            }
        }
    }
}