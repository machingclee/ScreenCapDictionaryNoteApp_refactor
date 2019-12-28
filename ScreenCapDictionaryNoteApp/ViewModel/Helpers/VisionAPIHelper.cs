using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using Grpc.Auth;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class VisionAPIHelper
    {

        public static GoogleCredential Credential = GoogleCredential.FromFile(".\\environment_variable.json");

        public static Task<IReadOnlyList<Google.Cloud.Vision.V1.EntityAnnotation>> TextDetectionAsync(byte[] img_byte)
        {
            var newTask = Task.Factory.StartNew(async () =>
            {
                IReadOnlyList<Google.Cloud.Vision.V1.EntityAnnotation> response;
                Channel channel = new Channel(
                    ImageAnnotatorClient.DefaultEndpoint.Host,
                    ImageAnnotatorClient.DefaultEndpoint.Port,
                   Credential.ToChannelCredentials());
                ImageAnnotatorClient client = ImageAnnotatorClient.Create(channel);
                using (MemoryStream imageStream = new MemoryStream(img_byte))
                {
                    var image = Google.Cloud.Vision.V1.Image.FromStream(imageStream);
                    response = await client.DetectTextAsync(image);
                }
                return response;
            });

            return newTask.Result;

        }

    }
}
