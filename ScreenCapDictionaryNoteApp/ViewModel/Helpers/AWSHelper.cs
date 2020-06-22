
using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class AWSHelper
    {
        private static string bucketName = "screencapdicscreenshots";
        private static string AccessKey = Environment.GetEnvironmentVariable("AWSAccessKeyId");
        private static string SecretKey = Environment.GetEnvironmentVariable("AWSSecretKey");


        public AWSHelper()
        { }

        public static async void UploadFileAsync(string userName, string filePath)
        {
            try
            {
                using (var fileTransferUtility = new TransferUtility(
                     AccessKey,
                     SecretKey,
                     RegionEndpoint.APSoutheast1
                     ))
                {

                    await fileTransferUtility.UploadAsync(filePath, bucketName + "/" + userName);
                    Console.WriteLine("Upload completed");
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }


        public static async void RemoveFile(string fileName)
        {
            var client = new AmazonS3Client(
                       AccessKey,
                       SecretKey,
                       RegionEndpoint.APSoutheast1
                       );
            await client.DeleteObjectAsync(bucketName, fileName);
        }
    }
}
