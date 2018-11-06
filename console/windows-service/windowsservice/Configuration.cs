using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windowsservice
{
    public class Configuration : IConfiguration
    {
        // Get API key and proper url (if different region) from free Azure subscription, more: https://azure.microsoft.com/en-us/services/cognitive-services/face/
        public string SubscriptionKey { get; } = "";
        public string UriBase { get; } = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect";
        // default is in windows styled path, potential issue!
        public string ImagesPath { get; } = Path.Combine(@"D:\tty\files");
    }
}
