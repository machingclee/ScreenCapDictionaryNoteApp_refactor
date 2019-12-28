using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class TranslationAPIHelper
    {
        public static GoogleCredential Credential = GoogleCredential.FromFile(".\\environment_variable.json");

        public static TranslationResult Translate(string toBeTranslated)
        {

            TranslationClient client = TranslationClient.Create(Credential, TranslationModel.ServiceDefault);
            TranslationResult result = client.TranslateText(
                text: toBeTranslated,
                targetLanguage: "zh-TW",
                sourceLanguage: "ja",
                model: TranslationModel.NeuralMachineTranslation);
            Console.WriteLine($"Model: {result.Model}");
            Console.WriteLine(result.TranslatedText);
            return result;
        }

    }
}
