using System;
using System.IO;

namespace FaceSort
{
    public class Configuration : IConfiguration
    {
        public string SubscriptionKey { get; } = "";
        public string UriBase { get; } = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect";
        // default is in windows styled path, potential issue!
        public string ImagesPath { get; } = Path.Combine(@"D:\tty\files");
    }
}