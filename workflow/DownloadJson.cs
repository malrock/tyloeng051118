using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace demo
{

    public sealed class DownloadJson : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> ImageFile { get; set; }
        public InArgument<string> ApiUrl { get; set; }
        public InArgument<string> ApiKey { get; set; }
        public InArgument<string> JsonFile { get; set; }
        // Request parameters. A third optional parameter is "details".
        private const string RequestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                                                 "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                                                 "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            var imageFile = context.GetValue(this.ImageFile);
            var apiUrl = context.GetValue(this.ApiUrl);
            var apiKey = context.GetValue(this.ApiKey);
            var jsonPath = context.GetValue(this.JsonFile);
            Task.Run(()=>MakeAnalysisRequest(imageFile, apiKey, apiUrl,jsonPath)).Wait();
        }
                /// <summary>
        ///     Gets the analysis of the specified image by using the Face REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        public async Task MakeAnalysisRequest(string imageFilePath, string _subscriptionKey,string _uriBase,string _jsonFile)
        {
            var client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", _subscriptionKey);


            // Assemble the URI for the REST API Call.
            var uri = _uriBase + "?" + RequestParameters;

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
                var json = await response.Content.ReadAsStringAsync();
                using (var jsonFile = new StreamWriter(_jsonFile))
                {
                    jsonFile.WriteLine(json);    
                }
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
