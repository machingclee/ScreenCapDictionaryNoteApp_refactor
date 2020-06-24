using Newtonsoft.Json;
using ScreenCapDictionaryNoteApp.Model;
using ScreenCapDictionaryNoteApp.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class WebServiceHelper
    {
        private static string postRequestEndPoint = "http://64.227.19.181:8080/screencap/upload";
        private static string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MiwiaWF0IjoxNTkyOTM3NDg2LCJleHAiOjE1OTU1Mjk0ODZ9.0cF3x9hEt3ff6WoEmfp-2mK-LHF9wreJzoPZVm23mTI";

        public static event EventHandler OpenMessageBoxHandler;
        public static event EventHandler UpdateMessageBoxHandler;





        private void OpenMessageBox()
        {
            var messageBox = new Notification("***** Status *****");
            messageBox.OpenMessageBox();

        }

        private void UpdateMessageBox(String message)
        {
            UpdateMessageBoxHandler(this, new MessageBoxEventArgs(message));
        }

        public async void SyncToWebServer()
        {
            OpenMessageBox();

            try
            {
                UpdateMessageBox("Uploading ...");
                var processedPage = new List<string>();

                List<Note> allNotes = DatabaseHelper.Read<Note>();
                List<Page> allPages = DatabaseHelper.Read<Page>();
                List<Vocab> allVocabs = DatabaseHelper.Read<Vocab>();



                foreach (var page in allPages.ToList())
                {
                    if (page.CroppedScreenshotByteArray == null)
                    {
                        var list = allVocabs.RemoveAll(vocab => vocab.PageId == page.Id);
                        allPages.Remove(page);
                    }
                }
                // ------------------- add necessary update  ------------------- 

                var syncInfo = new SyncInfoModel()
                {
                    Notes = allNotes,
                    Pages = allPages,
                    Vocabs = allVocabs
                };


                UpdateMessageBox("Sending Images to S3 Bucket ...");
                foreach (Page page in allPages)
                {
                    if (!page.IsSyncToS3)
                    {
                        if (page.CroppedScreenshotByteArray != null)
                        {
                            AWSHelper.UploadFileAsync("cclee", page.CroppedScreenshotByteArray);
                            page.IsSyncToS3 = true;
                            DatabaseHelper.Update(page);
                        }
                    }
                }
                UpdateMessageBox("Done");



                var jsonStringForUpdate = JsonConvert.SerializeObject(syncInfo);


                using (var client = new HttpClient())
                {
                    UpdateMessageBox("Uploading Vocabs ...");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var updateContent = new StringContent(jsonStringForUpdate, Encoding.UTF8, "application/json");

                    try
                    {
                        var response = await client.PostAsync(postRequestEndPoint, updateContent);
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ResponseMessageModel>(jsonString);

                        UpdateMessageBox(result.message);
                    }
                    catch (Exception err)
                    {
                        Debug.WriteLine(err.Message);
                        UpdateMessageBox(err.Message);
                    }
                }
            }
            catch (Exception err)
            {

            }
        }



        public class MessageBoxEventArgs : EventArgs
        {
            public string Message { get; set; }
            public MessageBoxEventArgs(string msg)
            {
                Message = msg;
            }
        }
        private class SyncInfoModel
        {
            public List<Note> Notes { get; set; }
            public List<Page> Pages { get; set; }
            public List<Vocab> Vocabs { get; set; }
        }
    }
}
