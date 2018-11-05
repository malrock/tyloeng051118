using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;

namespace FaceSort
{
    internal static class Program
    {
        public static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();
        public const string SubscriptionKey = "";
        public const string UriBase = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect";
        // default is in windows styled path, potential issue!
        public static string ImagesPath  = Path.Combine(@"D:\koolitus\mon");

        private static void Main()
        {
            if (LogManager.Configuration != null)
            {
                Logger.Info("Configuration present, ignoring built-in defaults.");
                return;
            }

            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget {Name = "ColorConsole"};
            var highlightRule = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Info"),
                ForegroundColor = ConsoleOutputColor.Blue
            };
            consoleTarget.RowHighlightingRules.Add(highlightRule);
            var errorHighlightRule = new ConsoleRowHighlightingRule
            {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Error"),
                ForegroundColor = ConsoleOutputColor.Red
            };
            consoleTarget.RowHighlightingRules.Add(errorHighlightRule);

            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);
            LogManager.Configuration = config;
            Logger.Info("Initialized logger from built-in defaults.");
            Logger.Info("Starting up main program");
            Task.Run(Scan).Wait();
           
            Logger.Info("Execution done, shutdown.");
        }
        public static async Task Scan()
        {
            var baseDir = new DirectoryInfo(ImagesPath);
            var images = baseDir.GetFiles("*.jpg");
            foreach (var image in images)
            {
                Logger.Info("Found image {name}",image.Name);
                var jDataFile = new FileInfo(image.FullName.Replace(image.Extension, ".json"));
                if (jDataFile.Exists)
                {
                    Logger.Info("Json data file found, skipping.");
                    continue;
                }
                Logger.Info("Requesting face data from API");
                var json = await MakeAnalysisRequest(image.FullName);
                Logger.Info("Writing face data to json file {file}",jDataFile.Name);
                using (var jsonFile = new StreamWriter(jDataFile.FullName))
                {
                    jsonFile.WriteLine(json);    
                }
            }
        }
        // Request parameters. A third optional parameter is "details".
        private const string RequestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                                                 "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                                                 "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
        /// <summary>
        ///     Gets the analysis of the specified image by using the Face REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        public static async Task<string> MakeAnalysisRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", SubscriptionKey);


            // Assemble the URI for the REST API Call.
            var uri = UriBase + "?" + RequestParameters;

            // Request body. Posts a locally stored JPEG image.
            var byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                var response = await client.PostAsync(uri, content);

                // Get the JSON response.
                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        ///     Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (var fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                var binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int) fileStream.Length);
            }
        }
    }
}