
using Amazon;
using Amazon.Runtime.CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class AWSHelper
    {
        public AWSHelper()
        {
            var options = new CredentialProfileOptions
            {
                AccessKey = "",
                SecretKey = ""
            };
            var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile("basic_profile", options);
            profile.Region = RegionEndpoint.USWest1;
            var netSDKFile = new NetSDKCredentialsFile();
            netSDKFile.RegisterProfile(profile);
        }
    }
}
