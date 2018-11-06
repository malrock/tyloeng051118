using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace windowsservice
{
    class FaceApi
    {
        // Request parameters. A third optional parameter is "details".
        private const string RequestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                                                 "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                                                 "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

        private readonly string _subscriptionKey;
        private readonly string _uriBase;

        public FaceApi(IConfiguration conf)
        {
            _subscriptionKey = conf.SubscriptionKey;
            _uriBase = conf.UriBase;
        }

        /// <summary>
        ///     Gets the analysis of the specified image by using the Face REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        public async Task<string> MakeAnalysisRequest(string imageFilePath)
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
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

    }
}
