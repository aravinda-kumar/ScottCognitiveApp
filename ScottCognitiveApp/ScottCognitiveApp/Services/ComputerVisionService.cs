using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ScottCognitiveApp.Models.Image;
using Newtonsoft.Json;

namespace ScottCognitiveApp.Services
{
    /// <summary>
    /// Client for Computer Vision API (Microsoft Cognitive Services).
    /// </summary>
    public class ComputerVisionService
    {

        /// <summary>
        /// Get a subscription key from:
        /// https://www.microsoft.com/cognitive-services/en-us/subscriptions
        /// </summary>
        private readonly string _key = "2a55f74b65344ac7b9f00cde32bf40a7";

        /// <summary>
        /// Documentation for the API: https://www.microsoft.com/cognitive-services/en-us/computer-vision-api
        /// </summary>
        private readonly string _analyseImageUriOld = "https://api.projectoxford.ai/vision/v1.0/analyze?" + "visualFeatures=Description,Categories,Tags,Faces,ImageType,Color,Adult&details=Celebrities";

        private readonly string _analyseImageUri = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze?"+
            "visualFeatures=Description,Categories,Tags,Faces,ImageType,Color,Adult&details=Celebrities&language=en";


        /// <summary>
        /// Get a subscription key from:
        /// https://www.microsoft.com/cognitive-services/en-us/subscriptions
        /// </summary>
        /// <param name="key">subscription key: required to access the API</param>
        public ComputerVisionService(string key)
        {
            _key = key;
        }

        /// <summary>
        /// This operation extracts a rich set of visual features based on the image content. 
        /// </summary>
        /// <param name="stream">The image to be uploaded.</param>
        /// <returns></returns>
        public async Task<ImageResult> AnalyseImageStreamAsync(Stream stream)
        {

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);

            var streamContent = new StreamContent(stream);

            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            try
            {
                var response = await httpClient.PostAsync(_analyseImageUri, streamContent);

                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {

                    var imageResult = JsonConvert.DeserializeObject<ImageResult>(json);

                    return imageResult;
                }

                throw new Exception(json);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return null;
        }

        
    }
}
